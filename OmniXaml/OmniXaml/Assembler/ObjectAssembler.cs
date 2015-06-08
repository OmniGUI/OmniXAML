namespace OmniXaml.Assembler
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Glass;
    using NodeWriters;
    using TypeConversion;
    using Typing;

    public class ObjectAssembler : IObjectAssembler
    {        
        private readonly GetObjectWriter getObjectWriter;
        private readonly NamespaceWriter namespaceWriter;
        private readonly StartMemberWriter startMemberWriter;
        private readonly StartObjectWriter startObjectWriter;
        private readonly TypeOperations typeOperations;
        private readonly ValueWriter valueWriter;
        private readonly ITypeContext typeRepository;
        private readonly ITypeContext xamlTypeRepository;
        private readonly ITypeConverterProvider typeConverterProvider;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public ObjectAssembler(WiringContext wiringContext)
        {
            typeConverterProvider = wiringContext.ConverterProvider;
            xamlTypeRepository = wiringContext.TypeContext;
            typeRepository = wiringContext.TypeContext;            
            typeOperations = new TypeOperations(typeRepository.TypeFactory);
            startMemberWriter = new StartMemberWriter(this);
            getObjectWriter = new GetObjectWriter(this);
            startObjectWriter = new StartObjectWriter(this);
            valueWriter = new ValueWriter(this);
            namespaceWriter = new NamespaceWriter();            
        }

        internal StateBag Bag { get; } = new StateBag();

        public object Result { get; internal set; }
        public void OverrideInstance(object instance)
        {
            Bag.PushScope();
            Bag.Current.Instance = instance;
        }

        public void WriteNode(XamlNode node)
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
                    namespaceWriter.WriteNamespace(node.NamespaceDeclaration);
                    break;
                default:
                    throw new InvalidOperationException($"Cannot handle this kind of node type: {node.NodeType}");
            }
        }

        public void SetUnfinishedResult()
        {
            Result = null;
        }

        internal void PrepareNewInstanceBecauseWeWantToConfigureIt(StateBag bag)
        {
            var newInstance = typeOperations.Create(bag.Current.Type);
            bag.Current.Instance = newInstance;
        }

        public void PushScope()
        {
            Bag.PushScope();
        }

        public void WriteEndMember()
        {
            SetUnfinishedResult();
            var xamlMember = Bag.Current.Type != null ? Bag.Current.Property : Bag.Parent.Property;

            if (IsLeafMember)
            {
                var currentInstance = Bag.Current.Instance;
                var valueWasAssigned = true;

                if (currentInstance != null)
                {
                    var markupExtension = currentInstance as MarkupExtension;
                    if (markupExtension != null)
                    {
                        Bag.Current.Instance = markupExtension;
                        var xamlType = XamlTypeRepository.Get(currentInstance.GetType());

                        if (!xamlType.CanAssignTo(xamlMember.Type))
                        {
                            AssignValueFromMarkupExtension(Bag);
                            valueWasAssigned = false;
                        }
                    }
                    else
                    {
                        var xamlTypeOfCurrentInstance = XamlTypeRepository.Get(currentInstance.GetType());

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

        private void AssignCurrentInstanceToParent(StateBag bag)
        {
            var parentProperty = bag.Parent.Property;
            var currentInstance = bag.Current.Instance;
            var parentPropertyType = parentProperty.Type;

            if (IsAssignmentBeingMadeToContainer(parentProperty, parentPropertyType))
            {
                AssignInstanceToParentCollection(bag.Parent.Collection, bag.Current.Instance);
            }
            else if (bag.Parent.Instance != null)
            {              
                if (!bag.Current.IsObjectFromMember)
                {
                    ApplyPropertyValue(bag, parentProperty, currentInstance, true);
                }
            }
        }

        public void WriteEndObject()
        {
            if (!Bag.Current.IsObjectFromMember)
            {
                if (Bag.Current.Instance == null)
                {
                    PrepareNewInstanceBecauseWeWantToConfigureIt(Bag);
                }

                if (IsMarkupExtension(Bag.Current.Type))
                {
                    AssignValueFromMarkupExtension(Bag);
                }
            }

            if (Bag.LiveDepth > 1)
            {
                AssignCurrentInstanceToParent(Bag);
            }

            Result = Bag.Current.Instance;

            Bag.PopScope();
        }

        // TODO: This may be a duplicate (EndMemberWriter also uses it)
        private void AssignValueFromMarkupExtension(StateBag stateBag)
        {
            var markupExtension = (MarkupExtension)stateBag.Current.Instance;

            var extensionContext = GetExtensionContext(stateBag);

            var providedValue = markupExtension.ProvideValue(extensionContext);

            stateBag.Current.Type = typeRepository.Get(providedValue.GetType());
            stateBag.Current.Instance = providedValue;

            AssignCurrentInstanceToParent(Bag);
        }

        private static MarkupExtensionContext GetExtensionContext(StateBag stateBag)
        {
            var inflationContext = new MarkupExtensionContext
            {
                TargetObject = stateBag.Parent.Instance,
                TargetProperty = stateBag.Parent.Instance.GetType().GetRuntimeProperty(stateBag.Parent.Property.Name),
            };

            return inflationContext;
        }

        private static bool IsMarkupExtension(XamlType type)
        {
            var underlyingType = type.UnderlyingType.GetTypeInfo();

            var meType = typeof(MarkupExtension).GetTypeInfo();
            return meType.IsAssignableFrom(underlyingType);
        }

        public static bool IsAssignmentBeingMadeToContainer(XamlMember parentProperty, XamlType type)
        {
            return parentProperty.IsDirective && type.IsContainer;
        }

        public static void AssignInstanceToParentCollection(object parentCollection, object instance)
        {
            TypeOperations.Add(parentCollection, instance);
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

        public bool IsLeafMember => Bag.Current.Type == null;

        public IXamlTypeRepository XamlTypeRepository
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
                var anotherValue = typeConverter.ConvertFrom(CultureInfo.InvariantCulture, value);
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