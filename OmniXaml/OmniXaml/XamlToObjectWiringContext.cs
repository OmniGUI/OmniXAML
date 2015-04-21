namespace OmniXaml
{
    using System.Reflection;

    public class XamlToObjectWiringContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }
}