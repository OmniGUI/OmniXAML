namespace OmniXaml
{
    using ObjectAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IRuntimeTypeSource typeSource;

        public DefaultObjectAssemblerFactory(IRuntimeTypeSource typeSource)
        {
            this.typeSource = typeSource;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new ObjectAssembler.ObjectAssembler(typeSource, new TopDownValueContext(), objectAssemblerSettings);
        }
    }
}