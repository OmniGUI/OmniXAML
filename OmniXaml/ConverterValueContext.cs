namespace OmniXaml
{
    using System;
    using TypeLocation;

    public class ConverterValueContext
    {
        public ObjectBuilderContext ObjectBuilderContext { get; }
        public Type TargetType { get; }
        public object Value { get; set; }
        public ITypeDirectory TypeDirectory { get; }
        public BuildContext BuildContext { get; set; }

        public ConverterValueContext(Type targetType, object value, ObjectBuilderContext objectBuilderContext, ITypeDirectory directory, BuildContext buildContext)
        {
            TargetType = targetType;
            Value = value;
            TypeDirectory = directory;
            BuildContext = buildContext;
            ObjectBuilderContext = objectBuilderContext;
        }
    }
}