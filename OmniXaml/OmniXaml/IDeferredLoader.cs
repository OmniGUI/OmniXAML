namespace OmniXaml
{
    using System.Collections.Generic;

    public interface IDeferredLoader
    {
        object Load(IEnumerable<XamlNode> nodes, WiringContext wiringContext);
    }
}