namespace OmniXaml.Parsers.MarkupExtensions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    [DebuggerDisplay("{ToString()}")]
    public class OptionsCollection : Collection<Option>
    {
        public OptionsCollection()
        {            
        }

        public OptionsCollection(IEnumerable<Option> options) : base(options.ToList())
        {            
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Positional fields: ");
            stringBuilder.Append(string.Join(", ", Items.OfType<PositionalOption>()));

            stringBuilder.Append(" | Property assignments: ");
            stringBuilder.Append(string.Join(", ", Items.OfType<PropertyOption>()));

            return stringBuilder.ToString();
        }

        protected bool Equals(OptionsCollection other)
        {
            return !other.Where((t, i) => !other.Items[i].Equals(Items[i])).Any();
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
            return Equals((OptionsCollection) obj);
        }

        public override int GetHashCode()
        {          
            if (Count == 0)
            {
                return base.GetHashCode();
            }

            int hashCode = Items[0].GetHashCode();
            int t = 1;
            while (t < Count)
            {
                hashCode ^= Items[t].GetHashCode();
            }

            return hashCode;
        }
    }
}