namespace OmniXaml
{
    using System.Reflection;
    using TypeConversion;

    public class MarkupExtensionContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
        public IValueContext ValueContext { get; set; }        
    }
}