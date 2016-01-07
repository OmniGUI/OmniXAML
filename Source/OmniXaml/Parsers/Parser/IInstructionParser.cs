namespace OmniXaml.Parsers.Parser
{
    using System.Collections.Generic;

    public interface IInstructionParser : IParser<IEnumerable<ProtoInstruction>, IEnumerable<Instruction>>
    {        
    }
}