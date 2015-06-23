namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class VisualizationNode
    {
        public XamlNode XamlNode { get; }

        public VisualizationNode(string name) : this(new XamlNode(XamlNodeType.None))
        {

        }

        public VisualizationNode(XamlNode xamlNode)
        {
            this.XamlNode = xamlNode;           
            this.Children = new Collection<VisualizationNode>();
        }

        public ICollection<VisualizationNode> Children { get; set; }
    }
}