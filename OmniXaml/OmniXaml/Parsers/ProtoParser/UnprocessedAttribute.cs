namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class UnprocessedAttributeBase
    {
        public PropertyLocator Locator { get; }
        public string Value { get; }

        protected UnprocessedAttributeBase(PropertyLocator locator, string value)
        {
            Locator = locator;
            Value = value;
        }
    }

    internal class UnprocessedAttribute : UnprocessedAttributeBase
    {
        public UnprocessedAttribute(PropertyLocator locator, string value) : base(locator, value)
        {
        }
    }
}