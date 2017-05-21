namespace OmniXaml.Services
{
    public class TwoPassesNodeAssembler : INodeAssembler
    {
        private readonly NodeAssembler assembler;

        public TwoPassesNodeAssembler(IInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
        {
            assembler = new NodeAssembler(instanceCreator, converter, memberAssigmentApplier);
        }

        public void Assemble(ConstructionNode node, ConstructionNode parent = null)
        {
            assembler.Assemble(node, parent);
            assembler.Assemble(node, parent);
        }
    }
}