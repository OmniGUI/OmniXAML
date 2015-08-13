namespace OmniXaml
{
    using Assembler;
    using NewAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IWiringContext IWiringContext;

        public DefaultObjectAssemblerFactory(IWiringContext IWiringContext)
        {
            this.IWiringContext = IWiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new ObjectAssembler(IWiringContext, new TopDownMemberValueContext(), objectAssemblerSettings);
        }
    }
}