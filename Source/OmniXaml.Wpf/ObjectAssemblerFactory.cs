namespace OmniXaml.Wpf
{
    using Assembler;

    internal class ObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IWiringContext wiringContext;

        public ObjectAssemblerFactory(IWiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings settings)
        {
            return new ObjectAssembler(wiringContext, settings);
        }
    }
}