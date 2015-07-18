namespace OmniXaml.Wpf
{
    using Assembler;

    internal class ObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly WiringContext wiringContext;

        public ObjectAssemblerFactory(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler GetAssembler(ObjectAssemblerSettings settings)
        {
            return new ObjectAssembler(wiringContext, settings);
        }
    }
}