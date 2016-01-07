namespace OmniXaml
{
    using ObjectAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IRuntimeTypeSource typeContext;

        public DefaultObjectAssemblerFactory(IRuntimeTypeSource typeContext)
        {
            this.typeContext = typeContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new ObjectAssembler.ObjectAssembler(typeContext, new TopDownValueContext(), objectAssemblerSettings);
        }
    }
}