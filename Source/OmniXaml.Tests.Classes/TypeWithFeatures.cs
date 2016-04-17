namespace OmniXaml.Tests.Classes
{
    using Attributes;
    using TypeConversion;
    using TypeConversion.BuiltInConverters;

    [ContentProperty("Sample")]
    [TypeConverter(typeof(StringTypeConverter))]
    public class TypeWithFeatures
    {
    }
}