namespace OmniXaml
{
    using System.Collections.Generic;
    using ObjectAssembler;
    using TypeConversion;

    public class DefaultObjectAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IRuntimeTypeSource typeSource;

        public DefaultObjectAssemblerFactory(IRuntimeTypeSource typeSource)
        {
            this.typeSource = typeSource;
        }

        public IObjectAssembler CreateAssembler(Settings settings)
        {
            var topDownValueContext = new TopDownValueContext();
            return new ObjectAssembler.ObjectAssembler(typeSource, new ValueContext(typeSource, topDownValueContext, new Dictionary<string, object>()), settings);
        }
    }
}