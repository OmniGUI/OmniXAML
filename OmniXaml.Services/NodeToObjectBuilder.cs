namespace OmniXaml.Services
{
    public class NodeToObjectBuilder : INodeToObjectBuilder
    {
        private readonly INodeAssembler assembler;

        public NodeToObjectBuilder(INodeAssembler assembler)
        {
            this.assembler = assembler;
        }

        public object Build(ConstructionNode node, BuilderContext context = null)
        {
            assembler.Assemble(node, this, null, context ?? new BuilderContext());            
            return node.Instance;
        }
    }
}