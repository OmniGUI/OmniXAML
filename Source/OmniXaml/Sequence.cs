namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Sequence<T> : Collection<T>
    {
        public Sequence()
        {            
        }

        public Sequence(IList<T> col) : base(col)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is Sequence<T>)
            {
                return Equals((Sequence<T>) obj);
            }

            return base.Equals(obj);
        }    

        private bool Equals(Sequence <T> hierarchizedXamlNodeCollection)
        {
            if (!Items.Any() && !hierarchizedXamlNodeCollection.Items.Any())
                return true;

            return hierarchizedXamlNodeCollection.SequenceEqual(Items);
        }

        public override int GetHashCode()
        {
            return Items.GetHashCode();
        }
    }
}