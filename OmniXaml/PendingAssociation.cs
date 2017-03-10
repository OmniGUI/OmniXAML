namespace OmniXaml
{
    public class PendingAssociation
    {
        public object Owner { get; }
        public object Parent { get; private set; }
        public KeyedInstance Child { get; private set; }

        public PendingAssociation(object owner, object parent, KeyedInstance child)
        {
            Owner = owner;
            Parent = parent;
            Child = child;
        }
    }
}