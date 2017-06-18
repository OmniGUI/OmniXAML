using System;
using System.Collections.Generic;
using System.Linq;

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
                var unresolvedNodes = GetUnresolvedNodes(constructionNode);
                throw new ParseException("XAML cannot be parsed due to nodes that cannot be resolved:" + string.Join(",", unresolvedNodes));
            }
            return build;
        }

        private IEnumerable<ConstructionNode> GetUnresolvedNodes(ConstructionNode constructionNode)
        {
            if (!constructionNode.IsCreated && SubNodesAreCreated(constructionNode))
            {
                yield return constructionNode;
            }

            foreach (var constructionNodeAssignment in constructionNode.Assignments)
            {
                var values = constructionNodeAssignment.Values;
                foreach (var value in values)
                {
                    foreach (var unresolved in GetUnresolvedNodes(value))
                    {
                        yield return unresolved;
                    }
                }
            }
        }

        private bool SubNodesAreCreated(ConstructionNode constructionNode)
        {
            var membersCreated = constructionNode.Assignments.All(ma => ma.Values.All(node => node.IsCreated));
            var childrenCreated = constructionNode.Assignments.All(ma => ma.Values.All(node => node.IsCreated));

            return membersCreated && childrenCreated;
        }
    }
}