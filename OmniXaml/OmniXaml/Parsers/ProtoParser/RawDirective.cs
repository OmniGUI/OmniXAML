namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class RawDirective : UnprocessedAttributeBase
    {
        public RawDirective(PropertyLocator locator, string value) : base(locator, value)
        {
        }
    }
}