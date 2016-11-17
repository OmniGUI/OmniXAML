namespace OmniXaml
{
    internal class ChildAssociation
    {
        public object Parent { get; private set; }
        public KeyedInstance Child { get; private set; }

        public ChildAssociation(object parent, KeyedInstance child)
        {
            Parent = parent;
            Child = child;
        }
    }
}