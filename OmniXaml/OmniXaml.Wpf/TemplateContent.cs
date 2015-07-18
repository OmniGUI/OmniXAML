namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows;

    public class TemplateContent
    {
        public IEnumerable<XamlNode> Nodes { get; }
        public WiringContext Context { get; }

        public TemplateContent(IEnumerable<XamlNode> nodes, WiringContext context)
        {
            this.Nodes = nodes;
            this.Context = context;
        }

        public FrameworkElement Load()
        {
            var loader = new ObjectAssembler(Context);
            foreach (var xamlNode in Nodes)
            {
                loader.Process(xamlNode);
            }

            return (FrameworkElement) loader.Result;
        }
    }
}