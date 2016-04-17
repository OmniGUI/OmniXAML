namespace OmniXaml.TypeConversion
{
    using System.Collections.Generic;
    using ObjectAssembler.Commands;
    using Typing;

    public class ValueContext : IValueContext
    {
        private readonly ITypeRepository typeRepository;
        private readonly ITopDownValueContext topDownValueContext;
        private readonly IReadOnlyDictionary<string, object> parsingDictionary;

        public ValueContext(ITypeRepository typeRepository, ITopDownValueContext topDownValueContext, IReadOnlyDictionary<string, object> parsingDictionary)
        {
            this.typeRepository = typeRepository;
            this.topDownValueContext = topDownValueContext;
            this.parsingDictionary = parsingDictionary;
        }
        
        public ITypeRepository TypeRepository => typeRepository;

        public ITopDownValueContext TopDownValueContext => topDownValueContext;
        public IReadOnlyDictionary<string, object> ParsingDictionary => parsingDictionary;
    }
}