namespace OmniXaml.Tests.Classes
{
    public class AnotherExtension : MarkupExtension
    {
        public string Property { get; set; }
        public string AnotherProperty { get; set; }

        public override object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return "Text From Markup Extension";
        }
    }
}