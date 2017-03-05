namespace OmniXaml
{
    public class ParentChildRelationship
    {
        public object Parent { get; }
        public object Child { get; }

        public ParentChildRelationship(object parent, object child)
        {
            Parent = parent;
            Child = child;
        }
    }
}