namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class DirectiveAssignment
    {
        private readonly Directive directive;
        private readonly string value;

        public DirectiveAssignment(Directive directive, string value)
        {
            this.directive = directive;
            this.value = value;
        }

        public Directive Directive => directive;

        public string Value => value;
    }
}