namespace AvaloniaApp.Context
{
    using System.Collections.Generic;
    using OmniXaml.ObjectAssembler.Commands;
    using OmniXaml.TypeConversion;
    using OmniXaml.Typing;

    public class HackedValueContext : IValueContext
    {
        public HackedValueContext()
        {
            ParsingDictionary = new Dictionary<string, object>() {{"Uri", "\\"}};
        }

        public ITypeRepository TypeRepository { get; }
        public ITopDownValueContext TopDownValueContext { get; }
        public IReadOnlyDictionary<string, object> ParsingDictionary { get; } 
    }
}