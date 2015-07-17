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

        public IObjectAssembler GetAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new SuperObjectAssembler(wiringContext, objectAssemblerSettings);
        }
    }
}