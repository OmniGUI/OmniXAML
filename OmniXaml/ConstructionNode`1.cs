namespace OmniXaml
{
    public class ConstructionNode<T> : ConstructionNode
    {
        public ConstructionNode() : base(typeof(T))
        {
        }
    }
}