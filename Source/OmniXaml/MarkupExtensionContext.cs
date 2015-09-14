namespace OmniXaml
{
    using System.Reflection;
    using ObjectAssembler.Commands;
    using Typing;

    public class MarkupExtensionContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
        public IXamlTypeRepository TypeRepository { get; set; }
        public ITopDownValueContext TopDownValueContext { get; set; }
    }
}