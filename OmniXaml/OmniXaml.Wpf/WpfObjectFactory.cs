namespace OmniXaml.Wpf
{
    using Assembler;

    internal class WpfObjectFactory : IObjectAssemblerFactory
    {
        private readonly WiringContext wiringContext;

        public WpfObjectFactory(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler GetAssembler(ObjectAssemblerSettings settings)
        {
            return new WpfObjectAssembler(wiringContext, settings);
        }
    }
}