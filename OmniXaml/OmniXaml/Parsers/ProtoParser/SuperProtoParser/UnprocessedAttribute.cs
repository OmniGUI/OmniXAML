namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using Typing;

    internal class UnprocessedAttribute 
    {
        public PropertyLocator Locator { get; }
        public string Value { get; }

        public UnprocessedAttribute(PropertyLocator locator, string value)
        {
            Locator = locator;
            Value = value;
        }
    }
}