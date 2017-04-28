namespace OmniXaml
{
    using ReworkPhases;

    public class FullObjectBuilder : IFullObjectBuilder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier memberAssigmentApplier;

        public FullObjectBuilder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
            this.memberAssigmentApplier = memberAssigmentApplier;
        }

        public object Build(ConstructionNode node)
        {
            var mainBuilder = new ReworkPhases.ObjectBuilder(instanceCreator, converter, memberAssigmentApplier);
            var unresolvedFixer = new ObjectBuilderSecondPass(converter);

            var inflatedNode = mainBuilder.Assemble(node);
            return unresolvedFixer.Fix(inflatedNode);
        }
    }
}