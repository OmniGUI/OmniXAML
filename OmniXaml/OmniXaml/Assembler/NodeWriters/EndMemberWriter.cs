namespace OmniXaml.Assembler.NodeWriters
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using TypeConversion;
    using Typing;

    public class EndMemberWriter
    {
        private readonly ObjectAssembler objectAssembler;
        private readonly ITypeConverterProvider typeConverterProvider;
        private readonly IXamlTypeRepository xamlTypeRepository;
        private readonly StateBag bag;

        public EndMemberWriter(ObjectAssembler objectAssembler,
            WiringContext wiringContext)
        {
            this.objectAssembler = objectAssembler;
            typeConverterProvider = wiringContext.ConverterProvider;
            xamlTypeRepository = wiringContext.TypeContext;
            bag = objectAssembler.Bag;
        }

        private bool IsLeafMember => bag.Current.Type == null;

        public void WriteEndMember()
        {
            objectAssembler.SetUnfinishedResult();
            var xamlMember = bag.Current.Type != null ? bag.Current.Property : bag.Parent.Property;

            if (IsLeafMember)
            {
                var currentInstance = bag.Current.Instance;
                var valueWasAssigned = true;

                if (currentInstance != null)
                {
                    var markupExtension = currentInstance as MarkupExtension;
                    if (markupExtension != null)
                    {
                        bag.Current.Instance = markupExtension;
                        var xamlType = xamlTypeRepository.Get(currentInstance.GetType());

                        if (!xamlType.CanAssignTo(xamlMember.Type))
                        {
                            AssignValueFromMarkupExtension(bag);
                            valueWasAssigned = false;
                        }
                    }
                    else
                    {
                        var xamlTypeOfCurrentInstance = xamlTypeRepository.Get(currentInstance.GetType());

                        var isString = Equals(xamlTypeOfCurrentInstance, CoreTypes.String);
                        var canAssign = xamlTypeOfCurrentInstance.CanAssignTo(xamlMember.Type);
                        if (isString || !canAssign)
                        {                            
                            valueWasAssigned = TryCreateCompatibleInstanceForCurrentMember(bag, bag.Current.Instance, xamlMember.Type.UnderlyingType);
                        }
                    }
                }

                objectAssembler.Result = bag.Current.Instance;

                if (valueWasAssigned)
                {
                    EndObjectWriter.AssignCurrentInstanceToParent(bag);
                }

                bag.PopScope();
            }

            bag.Current.Property = null;
            bag.Current.IsPropertyValueSet = false;
        }

        private void AssignValueFromMarkupExtension(StateBag stateBag)
        {
            throw new NotImplementedException();
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