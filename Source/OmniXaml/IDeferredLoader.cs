namespace OmniXaml
{
    using System.Collections.Generic;

    public interface IDeferredLoader
    {
        object Load(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeSource);
    }
}