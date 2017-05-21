namespace OmniXaml.Services
{
    public class NodeToObjectBuilder : INodeToObjectBuilder
    {
        private readonly INodeAssembler assembler;

        public NodeToObjectBuilder(INodeAssembler assembler)
        {
            this.assembler = assembler;
        }

        public object Build(ConstructionNode node)
        {
            assembler.Assemble(node);            
            return node.Instance;
        }
    }
}