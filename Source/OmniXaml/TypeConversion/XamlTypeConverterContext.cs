namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public class XamlTypeConverterContext : IXamlTypeConverterContext
    {
        private readonly ITypeRepository typeRepository;
        private readonly ITopDownValueContext topDownValueContext;

        public XamlTypeConverterContext(ITypeRepository typeRepository, ITopDownValueContext topDownValueContext)
        {
            this.typeRepository = typeRepository;
            this.topDownValueContext = topDownValueContext;
        }
        
        public ITypeRepository TypeRepository => typeRepository;

        public ITopDownValueContext TopDownValueContext => topDownValueContext;
    }
}