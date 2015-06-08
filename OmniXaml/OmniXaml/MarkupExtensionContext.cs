namespace OmniXaml
{
    using System.Reflection;

    public class MarkupExtensionContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }
}