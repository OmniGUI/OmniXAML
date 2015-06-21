namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;

    public class WpfTemplateContent
    {
        public IEnumerable<XamlNode> Nodes { get; }
        public WiringContext Context { get; }

        public WpfTemplateContent(IEnumerable<XamlNode> nodes, WiringContext context)
        {
            this.Nodes = nodes;
            this.Context = context;
        }

        public FrameworkElement Load()
        {
            var loader = new WpfObjectAssembler(Context);
            foreach (var xamlNode in Nodes)
            {
                loader.WriteNode(xamlNode);
            }

            return (FrameworkElement) loader.Result;
        }
    }
}