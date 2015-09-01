namespace OmniXaml.Parsers.XamlInstructions
{
    using System.Collections.Generic;

    public interface IXamlInstructionParser : IParser<IEnumerable<ProtoXamlInstruction>, IEnumerable<XamlInstruction>>
    {        
    }
}