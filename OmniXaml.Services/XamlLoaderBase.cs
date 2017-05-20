using OmniXaml.ReworkPhases;

namespace OmniXaml.Services
{
    public abstract class XamlLoaderBase : IXamlLoader
    {
        public abstract IXamlToTreeParser Parser { get; }
        public IMemberAssigmentApplier AssignmentApplier => new MemberAssigmentApplier(new NoActionValuePipeline());

        public abstract ISmartInstanceCreator SmartInstanceCreator { get; }
        public abstract IStringSourceValueConverter StringSourceValueConverter { get; }

        public virtual INodeToObjectBuilder Builder => new NodeToObjectBuilder(SmartInstanceCreator,
            StringSourceValueConverter, AssignmentApplier);

        public object Load(string xaml, object intance = null)
        {
            var prefixAnnotator = new PrefixAnnotator();
            var constructionNode = Parser.Parse(xaml, prefixAnnotator).Root;
            return Builder.Build(constructionNode);
        }
    }
}