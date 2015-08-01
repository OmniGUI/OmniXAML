namespace OmniXaml
{
    using Assembler;
    using NewAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly WiringContext wiringContext;

        public DefaultObjectAssemblerFactory(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new SuperObjectAssembler(wiringContext, new TopDownMemberValueContext(), objectAssemblerSettings);
        }
    }
}