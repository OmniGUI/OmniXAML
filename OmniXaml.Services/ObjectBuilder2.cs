namespace OmniXaml.Services
{
    using ReworkPhases;

    public class ObjectBuilder2 : IObjectBuilder2
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;

        public ObjectBuilder2(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
        }

        public object Inflate(ConstructionNode ctNode)
        {
            var mainBuilder = new Phase1Builder(instanceCreator, converter);
            var unresolvedFixer = new Phase2Builder(converter);

            var inflatedNode = mainBuilder.Inflate(ctNode);
            return unresolvedFixer.Fix(inflatedNode);
        }
    }


}