namespace OmniXaml
{
    using System.Collections.Generic;

    public interface IDeferredObjectAssembler
    {
        object Load(IEnumerable<XamlNode> nodes);
    }
}