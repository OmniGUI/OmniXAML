namespace OmniXaml.TypeConversion
{
    using Typing;

    public class XamlTypeConverterContext : IXamlTypeConverterContext
    {
        public XamlTypeConverterContext(IXamlTypeRepository typeRepository)
        {
            TypeRepository = typeRepository;
        }
        public IXamlTypeRepository TypeRepository { get;  }
    }
}