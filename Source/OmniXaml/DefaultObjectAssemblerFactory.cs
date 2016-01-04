namespace OmniXaml
{
    using ObjectAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly ITypeContext typeContext;

        public DefaultObjectAssemblerFactory(ITypeContext typeContext)
        {
            this.typeContext = typeContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new ObjectAssembler.ObjectAssembler(typeContext, new TopDownValueContext(), objectAssemblerSettings);
        }
    }
}