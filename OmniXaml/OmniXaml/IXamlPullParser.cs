namespace OmniXaml
{
    using System.Collections.Generic;

    public interface IXamlPullParser
    {
        IEnumerable<XamlNode> Nodes();
    }
}