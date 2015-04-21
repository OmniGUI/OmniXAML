namespace OmniXaml.Parsers.MarkupExtensions
{
    public class MarkupExtensionNode : TreeNode
    {
        public OptionsCollection Options { get; }
        public IdentifierNode Identifier { get; }

        public MarkupExtensionNode(IdentifierNode identifier) : this(identifier, new OptionsCollection())
        {            
        }

        public MarkupExtensionNode(IdentifierNode identifier, OptionsCollection options)
        {
            Options = new OptionsCollection(options);
            Identifier = identifier;
        }

        public override string ToString()
        {
            var optionString = Options?.ToString() ?? "no options";
            return $"MarkupExtension named {Identifier} with these options => {optionString}";
        }

        protected bool Equals(MarkupExtensionNode other)
        {
            return Equals(Options, other.Options) && Equals(Identifier, other.Identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((MarkupExtensionNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Options?.GetHashCode() ?? 0)*397) ^ Identifier.GetHashCode();
            }
        }
    }
}