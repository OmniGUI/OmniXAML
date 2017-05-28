namespace OmniXaml.Services
{
    public class TwoPassesNodeAssembler : INodeAssembler
    {
        private readonly NodeAssembler assembler;

        public TwoPassesNodeAssembler(IInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
        {
            assembler = new NodeAssembler(instanceCreator, converter, memberAssigmentApplier);
        }

        public void Assemble(ConstructionNode node, INodeToObjectBuilder nodeToObjectBuilder, ConstructionNode parent = null, BuilderContext context = null)
        {
            assembler.Assemble(node, nodeToObjectBuilder, parent, context);
            assembler.Assemble(node, nodeToObjectBuilder, parent, context);
        }
    }
}