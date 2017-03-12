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
        public IBuildContext BuildContext { get; set; }

        public ConverterValueContext(Type targetType, object value, ObjectBuilderContext objectBuilderContext, ITypeDirectory directory, IBuildContext buildContext)
        {
            TargetType = targetType;
            Value = value;
            TypeDirectory = directory;
            BuildContext = buildContext;
            ObjectBuilderContext = objectBuilderContext;
        }
    }
}