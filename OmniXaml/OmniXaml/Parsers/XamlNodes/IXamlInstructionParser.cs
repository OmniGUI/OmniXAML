namespace OmniXaml.Parsers.XamlNodes
{
    using System.Collections.Generic;
    using ProtoParser;

    public interface IXamlInstructionParser : IParser<IEnumerable<ProtoXamlInstruction>, IEnumerable<XamlInstruction>>
    {        
    }
}