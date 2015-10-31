namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using System.IO;

    public interface IProtoParser : IParser<IXmlReader, IEnumerable<ProtoXamlInstruction>>
    {        
    }
}