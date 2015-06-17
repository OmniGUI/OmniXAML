namespace OmniXaml.Wpf.Loader
{
    using System.Collections.Generic;

    public class DeferredObjectAssembler : IDeferredObjectAssembler
    {
        public object Load(IEnumerable<XamlNode> nodes, WiringContext context)
        {
            return new WpfTemplateContent(nodes, context);
        }
    }
}