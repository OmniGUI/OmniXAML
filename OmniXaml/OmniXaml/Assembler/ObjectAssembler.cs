namespace OmniXaml.Assembler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Glass;
    using NodeWriters;
    using TypeConversion;
    using TypeConversion.BuiltInConverters;
    using Typing;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly WiringContext wiringContext;
        private readonly GetObjectWriter getObjectWriter;
        private readonly StartMemberWriter startMemberWriter;
        private readonly StartObjectWriter startObjectWriter;
        private readonly TypeOperations typeOperations;
        private readonly ValueWriter valueWriter;
        private readonly ITypeContext typeRepository;
        private readonly ITypeContext xamlTypeRepository;
        private readonly ITypeConverterProvider typeConverterProvider;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public ObjectAssembler(WiringContext wiringContext, ObjectAssemblerSettings settings = null)
        {
            this.wiringContext = wiringContext;
            typeConverterProvider = wiringContext.ConverterProvider;
            xamlTypeRepository = wiringContext.TypeContext;
            typeRepository = wiringContext.TypeContext;
            typeOperations = new TypeOperations(typeRepository.TypeFactory);
            startMemberWriter = new StartMemberWriter(this);
            getObjectWriter = new GetObjectWriter(this);
            startObjectWriter = new StartObjectWriter(this, settings?.RootInstance);
            valueWriter = new ValueWriter(this);
        }

        internal StateBag Bag { get; } = new StateBag();

        public object Result { get; internal set; }
        public void OverrideInstance(object instance)
        {
            Bag.PushScope();
            Bag.Current.Instance = instance;
        }

        public WiringContext WiringContext => wiringContext;

        public void Process(XamlNode node)
        {
            Guard.ThrowIfNull(node, nameof(node));

            switch (node.NodeType)
            {
                case XamlNodeType.None:
                    break;
                case XamlNodeType.StartObject:
                    startObjectWriter.WriteStartObject(node.XamlType);
                    break;
                case XamlNodeType.EndObject:
                    WriteEndObject();
                    break;
                case XamlNodeType.StartMember:
                    startMemberWriter.WriteStartMember(node.Member);
                    break;
                case XamlNodeType.EndMember:
                    WriteEndMember();
                    break;
                case XamlNodeType.Value:
                    valueWriter.WriteValue(node.Value);
                    break;
                case XamlNodeType.GetObject:
                    getObjectWriter.WriteGetObject();
                    break;
                case XamlNodeType.NamespaceDeclaration:
                    WriteNamespace(node.NamespaceDeclaration);
                    break;
                default:
                    throw new InvalidOperationException($"Cannot handle this kind of node type: {node.NodeType}");
            }
        }

        private void WriteNamespace(NamespaceDeclaration namespaceDeclarationNode)
        {
            if (Bag.Current.Type != null)
            {
                Bag.PushScope();
            }

            var prefix = namespaceDeclarationNode.Prefix;
            var ns = namespaceDeclarationNode.Namespace;
            xamlTypeRepository.RegisterPrefix(new PrefixRegistration(prefix, ns));
        }

        public void SetUnfinishedResult()
        {
            Result = null;
        }

        internal void PrepareNewInstanceBecauseWeWantToConfigureIt(StateBag bag)
        {
            var parameters = bag.Current.ConstructorArguments != null ? bag.Current.ConstructorArguments.ToArray() : null;
            var newInstance = typeOperations.Create(bag.Current.Type, parameters);

            // Resets the constructor arguments
            bag.Current.ConstructorArguments = null;
            bag.Current.Instance = newInstance;


            if (bag.LiveDepth > 1 && !(newInstance is IMarkupExtension) && bag.LiveDepth > 1)
            {
                CheckAssignmentToParentStart(bag);
            }
        }

        private void CheckAssignmentToParentStart(StateBag bag)
        {
            var parentPropertyIsItemsDictionary = bag.Parent.Property == CoreTypes.Items && bag.Parent.Type.IsDictionary;
            if (!parentPropertyIsItemsDictionary)
            {
                bag.Current.WasAssignedAtCreation = true;
                AssignCurrentInstanceToParent(bag);
            }
            else
            {
                bag.Current.WasAssignedAtCreation = false;
            }
        }

        private void WriteEndMember()
        {
            SetUnfinishedResult();
            var xamlMember = Bag.Current.Type != null ? Bag.Current.Property : Bag.Parent.Property;

            if (Equals(xamlMember, CoreTypes.MarkupExtensionArguments))
            {
                ConvertCurrentCollectionToMarkupExtensionArguments();
            }
            else if (IsLeafMember)
            {
                var currentInstance = Bag.Current.Instance;
                var valueWasAssigned = true;

                if (currentInstance != null)
                {
                    var markupExtension = currentInstance as MarkupExtension;
                    if (markupExtension != null)
                    {
                        Bag.Current.Instance = markupExtension;
                        var xamlType = XamlTypeRepository.GetXamlType(currentInstance.GetType());

                        if (!xamlType.CanAssignTo(xamlMember.Type))
                        {
                            AssignValueFromMarkupExtension(Bag);
                            valueWasAssigned = false;
                        }
                    }
                    else
                    {
                        var xamlTypeOfCurrentInstance = XamlTypeRepository.GetXamlType(currentInstance.GetType());

                        var isString = Equals(xamlTypeOfCurrentInstance, CoreTypes.String);
                        var canAssign = xamlTypeOfCurrentInstance.CanAssignTo(xamlMember.Type);
                        if (isString || !canAssign)
                        {
                            valueWasAssigned = TryCreateCompatibleInstanceForCurrentMember(Bag, Bag.Current.Instance, xamlMember.Type.UnderlyingType);
                        }
                    }
                }

                Result = Bag.Current.Instance;

                if (valueWasAssigned)
                {
                    AssignCurrentInstanceToParent(Bag);
                }

                Bag.PopScope();
            }

            Bag.Current.Property = null;
            Bag.Current.IsPropertyValueSet = false;
        }

        private void ConvertCurrentCollectionToMarkupExtensionArguments()
        {
            var arguments = (List<MarkupExtensionArgument>)Bag.Current.Collection;

            var inflatedArguments = new List<object>();

            var xamlTypes = GetTypesOfBestCtorMatch(Bag.Current.Type, arguments.Count);

            int i = 0;
            foreach (var markupExtensionArgument in arguments)
            {
                var targetType = xamlTypes[i];
                var compatibleValue = ConvertValueIfNecessary(markupExtensionArgument.Value, targetType.UnderlyingType);
                inflatedArguments.Add(compatibleValue);
            }

            Bag.Current.ConstructorArguments = inflatedArguments;
        }

        private IList<XamlType> GetTypesOfBestCtorMatch(XamlType xamlType, int count)
        {
            var constructor = SelectConstructor(xamlType, count);
            return constructor.GetParameters().Select(p => typeRepository.GetXamlType(p.ParameterType)).ToList();
        }

        private ConstructorInfo SelectConstructor(XamlType xamlType, int count)
        {
            return xamlType.UnderlyingType.GetTypeInfo().DeclaredConstructors.First(info => info.GetParameters().Count() == count);
        }

        internal void AssignCurrentInstanceToParent(StateBag bag)
        {
            var parentProperty = bag.Parent.Property;
            var currentInstance = bag.Current.Instance;
            var parentPropertyType = parentProperty.Type;

            if (IsAssignmentBeingMadeToContainer(parentProperty, parentPropertyType))
            {
                AssignCurrentInstanceToParentCollection();
            }
            else if (bag.Parent.Instance != null)
            {
                if (!bag.Current.IsCollectionHoldingObject)
                {
                    ApplyPropertyValue(bag, parentProperty, currentInstance, true);
                }
            }
        }

        private void WriteEndObject()
        {
            if (!Bag.Current.IsCollectionHoldingObject)
            {
                if (Bag.Current.Instance == null)
                {
                    PrepareNewInstanceBecauseWeWantToConfigureIt(Bag);
                }

                if (IsMarkupExtension(Bag.Current.Type))
                {
                    AssignValueFromMarkupExtension(Bag);
                }
                else
                {
                    if (Bag.LiveDepth > 1 && !Bag.Current.WasAssignedAtCreation)
                    {
                        AssignCurrentInstanceToParent(Bag);
                    }
                }
            }

            Result = Bag.Current.Instance;

            Bag.PopScope();
        }

        private void AssignValueFromMarkupExtension(StateBag stateBag)
        {
            var markupExtension = (IMarkupExtension)stateBag.Current.Instance;

            var extensionContext = GetExtensionContext(stateBag);

            var providedValue = markupExtension.ProvideValue(extensionContext);

            stateBag.Current.Type = typeRepository.GetXamlType(providedValue.GetType());
            stateBag.Current.Instance = providedValue;

            AssignCurrentInstanceToParent(Bag);
        }

        private MarkupExtensionContext GetExtensionContext(StateBag stateBag)
        {
            var inflationContext = new MarkupExtensionContext
            {
                TargetObject = stateBag.Parent.Instance,
                TargetProperty = stateBag.Parent.Instance.GetType().GetRuntimeProperty(stateBag.Parent.Property.Name),
                TypeRepository = this.XamlTypeRepository,
            };

            return inflationContext;
        }

        private static bool IsMarkupExtension(XamlType type)
        {
            var underlyingType = type.UnderlyingType.GetTypeInfo();

            var meType = typeof(IMarkupExtension).GetTypeInfo();
            return meType.IsAssignableFrom(underlyingType);
        }

        private static bool IsAssignmentBeingMadeToContainer(XamlMember parentProperty, XamlType type)
        {
            return parentProperty.IsDirective && type.IsContainer;
        }

        internal void AssignCurrentInstanceToParentCollection()
        {
            var parentCollection = Bag.Parent.Collection;
            TypeOperations.Add(parentCollection, Bag.Current.Instance);
        }

        private void ApplyPropertyValue(StateBag bag, XamlMember parentProperty, object value, bool onParent)
        {
            var instance = onParent ? bag.Parent.Instance : bag.Current.Instance;

            var isSetExternally = OnSetValue(instance, parentProperty, value);

            if (!isSetExternally)
            {
                TypeOperations.SetValue(instance, parentProperty, value);
            }
        }

        private bool OnSetValue(object instance, XamlMember parentProperty, object value)
        {
            if (XamlSetValueHandler == null)
            {
                return false;
            }
            var e = new XamlSetValueEventArgs(parentProperty, value);
            XamlSetValueHandler(instance, e);
            return e.Handled;
        }

        private bool IsLeafMember => Bag.Current.Type == null;

        private IXamlTypeRepository XamlTypeRepository
        {
            get { return xamlTypeRepository; }
        }

        private bool TryCreateCompatibleInstanceForCurrentMember(StateBag stateBag, object instanceToConvert, Type destinationType)
        {
            var convertedValue = ConvertValueIfNecessary(instanceToConvert, destinationType);

            stateBag.Current.Instance = convertedValue;
            return true;
        }

        private object ConvertValueIfNecessary(object value, Type targetType)
        {
            object converted;
            var success = TrySpecialConversion(value, targetType, out converted);
            if (success)
            {
                return converted;
            }

            var typeConverter = typeConverterProvider.GetTypeConverter(targetType);
            if (typeConverter != null)
            {
                var context = new XamlTypeConverterContext(xamlTypeRepository);
                var anotherValue = typeConverter.ConvertFrom(context, CultureInfo.InvariantCulture, value);
                return anotherValue;
            }

            throw new ValueConversionException($"Cannot convert {value} to type {targetType}");
        }

        private static bool TrySpecialConversion(object value, Type targetType, out object converted)
        {
            var type = value.GetType();

            if (type == typeof(string) && targetType == typeof(string))
            {
                converted = value;
                return true;
            }

            if (type == typeof(string) && targetType == typeof(object))
            {
                converted = value.ToString();
                return true;
            }

            if (targetType.GetTypeInfo().IsEnum)
            {
                converted = Enum.Parse(targetType, value.ToString());
                return true;
            }

            converted = null;
            return false;
        }

        private static bool IsMarkupExtension(object currentInstance) => (currentInstance as MarkupExtension) == null;
    }
}