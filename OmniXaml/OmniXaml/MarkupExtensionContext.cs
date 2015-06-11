namespace OmniXaml
{
    using System.Reflection;
    using Typing;

    public class MarkupExtensionContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
        public IXamlTypeRepository TypeRepository { get; set; }
    }
}