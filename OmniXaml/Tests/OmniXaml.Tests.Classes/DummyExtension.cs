namespace OmniXaml.Tests.Classes
{
    public class DummyExtension : MarkupExtension
    {
        public DummyExtension()
        {            
        }

        public DummyExtension(string option)
        {
            this.Option = option;
        }

        public string Option { get; set; }

        public string Property { get; set; }
        public string AnotherProperty { get; set; }

        public override object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            if (Option != null)
            {
                return Option;
            }

            if (Property != null)
            {
                return Property;
            }

            return  "Text From Markup Extension";
        }
    }
}