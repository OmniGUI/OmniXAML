namespace OmniXaml
{
    using System;

    public interface IInlineParser
    {

        bool CanParse(string inline);
        ConstructionNode Parse(string inline, Func<string, string> prefixResolver);
    }
}