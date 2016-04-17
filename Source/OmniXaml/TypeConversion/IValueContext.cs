namespace OmniXaml.TypeConversion
{
    using System.Collections.Generic;
    using ObjectAssembler.Commands;
    using Typing;

    public interface IValueContext
    {
        ITypeRepository TypeRepository { get; }
        ITopDownValueContext TopDownValueContext { get;}
        IReadOnlyDictionary<string, object> ParsingDictionary { get; }
    }
}