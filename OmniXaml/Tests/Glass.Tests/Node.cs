namespace Glass.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Node : IDependency<Node>
    {
        public Node(string name) : this(name, new Collection<Node>())
        {
        }
        public Node(string name, IEnumerable<Node> collection)
        {
            Name = name;
            Dependencies = collection;
        }

        public string Name { get; set; }

        public IEnumerable<Node> Dependencies { get; set; }

        public override string ToString()
        {
            return Name;
        }

        protected bool Equals(Node other)
        {
            return string.Equals(Name, other.Name);
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
            return Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}