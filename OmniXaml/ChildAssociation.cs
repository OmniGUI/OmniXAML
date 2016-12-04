namespace OmniXaml
{
    public class ChildAssociation
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