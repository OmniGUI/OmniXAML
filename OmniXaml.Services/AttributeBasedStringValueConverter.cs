namespace OmniXaml.Services
{
    using System.Collections.Generic;
    using System.Reflection;

    public class AttributeBasedStringValueConverter : AttributeBasedValueConverter<string, object>, IStringSourceValueConverter
    {
        public AttributeBasedStringValueConverter(IList<Assembly> assemblies) : base(assemblies)
        {
        }
    }
}