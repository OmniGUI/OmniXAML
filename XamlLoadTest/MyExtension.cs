namespace XamlLoadTest
{
    using OmniXaml;

    public class SampleExtension : IMarkupExtension
    {
        public object GetValue(ExtensionValueContext context)
        {
            return "Hello from a Extension!";
        }
    }
}