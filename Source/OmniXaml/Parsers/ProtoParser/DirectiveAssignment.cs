namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class DirectiveAssignment
    {
        private readonly XamlDirective directive;
        private readonly string value;

        public DirectiveAssignment(XamlDirective directive, string value)
        {
            this.directive = directive;
            this.value = value;
        }

        public XamlDirective Directive => directive;

        public string Value => value;
    }
}