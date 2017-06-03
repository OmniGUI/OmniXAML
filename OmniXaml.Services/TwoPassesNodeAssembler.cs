namespace OmniXaml.Services
{
    public class TwoPassesNodeAssembler : INodeAssembler
    {
        private readonly NodeAssembler assembler;

        public TwoPassesNodeAssembler(NodeAssembler nodeAssembler)
        {
            assembler = nodeAssembler;
        }

        public void Assemble(ConstructionNode node, INodeToObjectBuilder nodeToObjectBuilder, ConstructionNode parent = null, BuilderContext context = null)
        {
            assembler.Assemble(node, nodeToObjectBuilder, parent, context);
            assembler.Assemble(node, nodeToObjectBuilder, parent, context);
        }
    }
}