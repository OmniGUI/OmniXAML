namespace OmniXaml
{
    using Assembler;
    using NewAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IWiringContext wiringContext;

        public DefaultObjectAssemblerFactory(IWiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new ObjectAssembler(wiringContext, new TopDownMemberValueContext(), objectAssemblerSettings);
        }
    }
}