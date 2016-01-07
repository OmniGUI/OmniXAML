namespace OmniXaml.TypeConversion
{
    using ObjectAssembler.Commands;
    using Typing;

    public class TypeConverterContext : ITypeConverterContext
    {
        private readonly ITypeRepository typeRepository;
        private readonly ITopDownValueContext topDownValueContext;

        public TypeConverterContext(ITypeRepository typeRepository, ITopDownValueContext topDownValueContext)
        {
            this.typeRepository = typeRepository;
            this.topDownValueContext = topDownValueContext;
        }
        
        public ITypeRepository TypeRepository => typeRepository;

        public ITopDownValueContext TopDownValueContext => topDownValueContext;
    }
}