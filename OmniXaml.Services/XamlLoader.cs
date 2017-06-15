namespace OmniXaml.Services
{
    public abstract class XamlLoader : IXamlLoader
    {
        public abstract IXamlToTreeParser Parser { get; }
        public abstract IMemberAssigmentApplier AssignmentApplier { get; }

        public abstract IInstanceCreator InstanceCreator { get; }
        public abstract IStringSourceValueConverter StringSourceValueConverter { get; }
        protected abstract IValuePipeline ValuePipeline { get; }

        public abstract INodeToObjectBuilder Builder { get; }

        public object Load(string xaml, object intance = null)
        {
            var prefixAnnotator = new PrefixAnnotator();
            var constructionNode = Parser.Parse(xaml, prefixAnnotator).Root;
            var build = Builder.Build(constructionNode);
            if (build == null)
            {
                throw new ParseException("XAML cannot be parsed");
            }
            return build;
        }
    }
}