namespace OmniXaml.Services
{
    using System.Collections.Generic;
    using System.Reflection;

    public class AttributeBasedStringValueConverter : AttributeBasedSmartSourceValueConverter<string, object>, IStringSourceValueConverter
    {
        public AttributeBasedStringValueConverter(IList<Assembly> assemblies) : base(assemblies)
        {
        }
    }
}