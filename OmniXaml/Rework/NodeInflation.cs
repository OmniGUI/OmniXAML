namespace OmniXaml.Rework
{
    public class NodeInflation
    {
        public NodeInflation(object creationResultInstance, ConstructionNode constructionNode)
        {
            Node = constructionNode;
            Instance = creationResultInstance;
        }

        public ConstructionNode Node { get; set; }
        public object Instance { get; set; }
    }
}