namespace OmniXaml
{
    public abstract class MarkupExtension : IMarkupExtension
    {
        public abstract object ProvideValue(XamlToObjectWiringContext toObjectWiringContext);        
    }
}