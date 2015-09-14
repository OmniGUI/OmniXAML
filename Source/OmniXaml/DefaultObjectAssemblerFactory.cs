namespace OmniXaml
{
    using ObjectAssembler;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IWiringContext wiringContext;

        public DefaultObjectAssemblerFactory(IWiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings objectAssemblerSettings)
        {
            return new ObjectAssembler.ObjectAssembler(wiringContext, new TopDownValueContext(), objectAssemblerSettings);
        }
    }
}