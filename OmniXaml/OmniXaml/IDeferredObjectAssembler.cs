namespace OmniXaml
{
    using System.Collections.Generic;
    using Tests;

    public interface IDeferredObjectAssembler
    {
        object Load(IEnumerable<XamlNode> nodes, WiringContext wiringContext);
    }
}