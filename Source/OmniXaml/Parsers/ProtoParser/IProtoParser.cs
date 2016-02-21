namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;

    public interface IProtoParser : IParser<IXmlReader, IEnumerable<ProtoInstruction>>
    {        
    }
}