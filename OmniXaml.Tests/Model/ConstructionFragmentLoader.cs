namespace OmniXaml.Tests.Model
{
    using Metadata;

    public class ConstructionFragmentLoader : IConstructionFragmentLoader
    {
        public object Load(ConstructionNode node, INodeToObjectBuilder builder, BuilderContext context)
        {
            return new TemplateContent(node, builder);
        }
    }
}