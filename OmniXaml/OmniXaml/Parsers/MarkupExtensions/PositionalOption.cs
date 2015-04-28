namespace OmniXaml.Parsers.MarkupExtensions
{
    using System.Diagnostics;

    [DebuggerDisplay("{ToString()}")]
    public class PositionalOption : Option
    {
        public string Identifier { get; }

        public PositionalOption(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return $"Identifier: {Identifier}";
        }

        protected bool Equals(PositionalOption other)
        {
            return string.Equals(Identifier, other.Identifier);
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
            return Equals((PositionalOption) obj);
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}