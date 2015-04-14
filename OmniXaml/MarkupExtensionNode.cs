namespace OmniXaml
{
    using System.Collections.Generic;

    public class MarkupExtensionNode
    {
        public OptionsCollection Options { get; }
        public string Identifier { get; }

        public MarkupExtensionNode(string identifier)
        {
            this.Identifier = identifier + "Extension";
        }

        public MarkupExtensionNode(string identifier, IEnumerable<Option> options) :  this(identifier)
        {
            Options = new OptionsCollection(options);
        }

        public override string ToString()
        {
            var optionString = Options?.ToString() ?? "no options";
            return $"MarkupExtension named {Identifier} with these options => {optionString}";
        }

        protected bool Equals(MarkupExtensionNode other)
        {
            return Equals(Options, other.Options) && string.Equals(Identifier, other.Identifier);
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
            if (obj.GetType() != this.GetType())
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