using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OmniXaml.Metadata;

namespace OmniXaml.Services
{
    public class ExtendedXamlLoader : BasicXamlLoader
    {
        private INodeToObjectBuilder builder;

        public ExtendedXamlLoader(IList<Assembly> assemblies) : base(assemblies)
        {
        }

        protected override IValuePipeline ValuePipeline =>
            new MarkupExtensionValuePipeline(new NoActionValuePipeline());

        public override INodeToObjectBuilder Builder => builder ??
                                                        (builder = new NodeToObjectBuilder(
                                                            new TwoPassesNodeAssembler(new TemplateAwareNodeAssembler(
                                                                InstanceCreator, StringSourceValueConverter,
                                                                AssignmentApplier, MetadataProvider))));
    }

    public class TemplateAwareNodeAssembler : NodeAssembler
    {
        private readonly IMetadataProvider metadataProvider;

        public TemplateAwareNodeAssembler(IInstanceCreator instanceCreator, IStringSourceValueConverter converter,
            IMemberAssigmentApplier assigmentApplier, IMetadataProvider metadataProvider) : base(instanceCreator,
            converter, assigmentApplier)
        {
            this.metadataProvider = metadataProvider;
        }

        protected override void AssembleFromChildren(MemberAssignment a, ConstructionNode constructionNode,
            INodeToObjectBuilder nodeToObjectBuilder, BuilderContext context)
        {
            var loaderInfo = metadataProvider.Get(constructionNode.ActualInstanceType).FragmentLoaderInfo;
            if (loaderInfo != null)
            {
                if (constructionNode.ActualInstanceType == loaderInfo.Type &&
                    a.Member.MemberName == loaderInfo.PropertyName)
                {
                    ConstructionNode payload = constructionNode.Assignments.First(assignment => assignment.Member.MemberName == loaderInfo.PropertyName).Values.First();
                    var loader = loaderInfo.Loader.Load(payload, nodeToObjectBuilder, context);
                    CreateInstance(constructionNode);
                    ApplyAssignments(constructionNode, nodeToObjectBuilder, context);
                    a.Values = new[] { new ConstructionNode(loader.GetType()) { Instance = loader } };
                    AssigmentApplier.ExecuteAssignment(new NodeAssignment(a, constructionNode.Instance), nodeToObjectBuilder, context);
                }
            }
            else
            {
                base.AssembleFromChildren(a, constructionNode, nodeToObjectBuilder, context);
            }
        }
    }

    public static class CloneMixin
    {
        public static ConstructionNode Clone(this ConstructionNode original)
        {
            return new ConstructionNode()
                {
                    InstantiateAs = original.InstantiateAs,
                    Instance = original.Instance,
                    IsCreated = original.IsCreated,
                    Name = original.Name,
                    Key = original.Key,
                    SourceValue = original.SourceValue,
                    PositionalParameters = original.PositionalParameters,
                    InstanceType = original.InstanceType,
                }.WithAssignments(original.Assignments.Select(a => a.Clone()).ToList())
                .WithChildren(original.Children.Select(node => node.Clone()));
        }

        public static MemberAssignment Clone(this MemberAssignment original)
        {
            return new MemberAssignment()
            {
                Member = original.Member,
                SourceValue = original.SourceValue,
                Values = original.Values.Select(n => n.Clone()).ToList(),
            };
        }
    }
}