namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class VisualizationNode
    {
        public XamlInstruction XamlInstruction { get; }

        public VisualizationNode(string name) : this(new XamlInstruction(XamlInstructionType.None))
        {

        }

        public VisualizationNode(XamlInstruction xamlInstruction)
        {
            this.XamlInstruction = xamlInstruction;           
            this.Children = new Collection<VisualizationNode>();
        }

        public ICollection<VisualizationNode> Children { get; set; }
    }
}