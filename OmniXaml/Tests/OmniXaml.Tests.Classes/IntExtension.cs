namespace OmniXaml.Tests.Classes
{
    public class IntExtension : MarkupExtension
    {
        public int Number { get; set; }       

        public override object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return Number;
        }
    }
}