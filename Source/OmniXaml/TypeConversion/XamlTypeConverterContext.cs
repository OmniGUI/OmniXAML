namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public class XamlTypeConverterContext : IXamlTypeConverterContext
    {
        private readonly IXamlTypeRepository typeRepository;
        private readonly ITopDownValueContext topDownValueContext;

        public XamlTypeConverterContext(IXamlTypeRepository typeRepository, ITopDownValueContext topDownValueContext)
        {
            this.typeRepository = typeRepository;
            this.topDownValueContext = topDownValueContext;
        }
        
        public IXamlTypeRepository TypeRepository => typeRepository;

        public ITopDownValueContext TopDownValueContext => topDownValueContext;
    }
}