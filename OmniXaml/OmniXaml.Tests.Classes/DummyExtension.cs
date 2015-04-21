namespace OmniXaml.Tests.Classes
{
    public class DummyExtension : MarkupExtension
    {
        public string Property { get; set; }
        public string AnotherProperty { get; set; }

        public override object ProvideValue(XamlToObjectWiringContext serviceProvider)
        {
            return Property ?? "Text From Markup Extension";
        }
    }
}