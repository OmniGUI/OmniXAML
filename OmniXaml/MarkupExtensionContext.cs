namespace OmniXaml
{
    public class MarkupExtensionContext
    {

        public ConstructionContext ConstructionContext { get; }
        public Assignment Assignment { get; }
        public ITypeDirectory TypeDirectory { get; }

        public MarkupExtensionContext(Assignment assignment, ConstructionContext constructionContext, ITypeDirectory directory)
        {
            TypeDirectory = directory;
            ConstructionContext = constructionContext;
            Assignment = assignment;
        }
    }
}