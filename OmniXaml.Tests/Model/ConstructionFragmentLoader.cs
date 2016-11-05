namespace OmniXaml.Tests.Model
{
    using Metadata;

    public class ConstructionFragmentLoader : IConstructionFragmentLoader
    {
        public object Load(ConstructionNode node, IObjectBuilder builder, BuildContext buildContext)
        {
            return new TemplateContent(node, builder, buildContext);
        }
    }
}