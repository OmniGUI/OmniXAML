namespace OmniXaml.Tests.Classes
{
    public class AnotherExtension : MarkupExtension
    {
        public string Property { get; set; }
        public string AnotherProperty { get; set; }

        public override object ProvideValue(XamlToObjectWiringContext toObjectWiringContext)
        {
            return "Text From Markup Extension";
        }
    }
}