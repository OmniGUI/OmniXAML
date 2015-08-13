namespace OmniXaml
{
    using System.Collections.Generic;

    public interface IDeferredLoader
    {
        object Load(IEnumerable<XamlInstruction> nodes, IWiringContext wiringContext);
    }
}