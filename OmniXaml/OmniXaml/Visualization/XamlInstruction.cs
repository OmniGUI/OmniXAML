namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Linq;

    public class HierarchizedXamlNode
    {
        public HierarchizedXamlNode()
        {
            Children = new Sequence<HierarchizedXamlNode>();
            Body = new Sequence<XamlInstruction>();
        }
        public XamlInstruction Leading { get; set; }
        public Sequence<HierarchizedXamlNode> Children { get; set; }
        public XamlInstruction Trailing { get; set; }
        public Sequence<XamlInstruction> Body { get; }

        public IEnumerable<XamlInstruction> Dump()
        {
            yield return Leading;

            foreach (var xamlNode in Body)
            {
                yield return xamlNode;
            }

            foreach (var xamlNode in Children.SelectMany(child => child.Dump()))
            {
                yield return xamlNode;
            }

            yield return Trailing;
        }

        protected bool Equals(HierarchizedXamlNode other)
        {
            var eq = Equals(Body, other.Body);
            return Leading.Equals(other.Leading) && Equals(Children, other.Children) && Trailing.Equals(other.Trailing) && eq;
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
            return Equals((HierarchizedXamlNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Leading.GetHashCode();
                hashCode = (hashCode*397) ^ Children.GetHashCode();
                hashCode = (hashCode*397) ^ Trailing.GetHashCode();
                hashCode = (hashCode*397) ^ Body.GetHashCode();
                return hashCode;
            }
        }

        public void AcceptVisitor(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public interface IVisitor
    {
        void Visit(HierarchizedXamlNode hierarchizedXamlNode);
    }
}