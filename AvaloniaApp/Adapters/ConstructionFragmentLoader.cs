namespace AvaloniaApp.Adapters
{
    using OmniXaml;
    using OmniXaml.Metadata;

    public class ConstructionFragmentLoader : IConstructionFragmentLoader
    {
        public object Load(ConstructionNode node, IObjectBuilder builder)
        {
            return new TemplateContent(node, builder);
        }
    }
}