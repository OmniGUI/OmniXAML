using OmniXaml;

namespace XamlLoadTest
{
    public class Reference : IMarkupExtension
    {
        public object GetValue(ExtensionValueContext context)
        {
            return null;
        }

        public ReferenceTarget Target { get; set; }
    }
}