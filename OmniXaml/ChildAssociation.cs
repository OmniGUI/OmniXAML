namespace OmniXaml
{
    internal class ChildAssociation
    {
        public object Parent { get; private set; }
        public object Child { get; private set; }
        public string Key { get; private set; }

        public ChildAssociation(object parent, object child, string key)
        {
            Parent = parent;
            Child = child;
            Key = key;
        }
    }
}