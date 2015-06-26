namespace OmniXaml.Tests.Classes
{
    public class IntExtension : MarkupExtension
    {
        public IntExtension()
        {            
        }

        public IntExtension(int number)
        {
            Number = number;
        }

        public int Number { get; set; }       

        public override object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return Number;
        }
    }
}