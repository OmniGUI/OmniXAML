namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class VisualizationNode
    {
        public Instruction Instruction { get; }

        public VisualizationNode(string name) : this(new Instruction(InstructionType.None))
        {

        }

        public VisualizationNode(Instruction instruction)
        {
            this.Instruction = instruction;           
            this.Children = new Collection<VisualizationNode>();
        }

        public ICollection<VisualizationNode> Children { get; set; }
    }
}