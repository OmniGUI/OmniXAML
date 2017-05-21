namespace OmniXaml.Services
{
    using ReworkPhases;

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

    public class TwoPassesNodeAssembler : INodeAssembler
    {
        private readonly NodeAssembler assembler;

        public TwoPassesNodeAssembler(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
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