namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Visualization;

    public class InstructionNode
    {
        public InstructionNode()
        {
            Children = new Sequence<InstructionNode>();
            Body = new Sequence<XamlInstruction>();
        }
        public XamlInstruction Leading { get; set; }
        public Sequence<InstructionNode> Children { get; set; }
        public XamlInstruction Trailing { get; set; }
        public Sequence<XamlInstruction> Body { get; }

        public IEnumerable<XamlInstruction> Dump()
        {
            yield return Leading;
            foreach (var instruction in Body) { yield return instruction; }
            foreach (var instruction in Children.SelectMany(child => child.Dump())) { yield return instruction; }
            yield return Trailing;
        }

        protected bool Equals(InstructionNode other)
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
            return Equals((InstructionNode) obj);
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
}