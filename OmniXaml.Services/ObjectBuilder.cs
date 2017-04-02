namespace OmniXaml.Services
{
    using ReworkPhases;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier memberAssigmentApplier;

        public ObjectBuilder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
            this.memberAssigmentApplier = memberAssigmentApplier;
        }

        public object Inflate(ConstructionNode ctNode)
        {
            var mainBuilder = new Phase1Builder(instanceCreator, converter, memberAssigmentApplier);
            var unresolvedFixer = new Phase2Builder(converter);

            var inflatedNode = mainBuilder.Inflate(ctNode);
            return unresolvedFixer.Fix(inflatedNode);
        }
    }
}