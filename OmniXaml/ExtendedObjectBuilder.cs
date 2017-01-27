namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Zafiro.Core;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly IContextFactory contextFactory;

        public ExtendedObjectBuilder(IInstanceCreator creator, ObjectBuilderContext objectBuilderContext, IContextFactory contextFactory)
            : base(creator, objectBuilderContext, contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        protected override void ApplyAssignments(object instance, IEnumerable<MemberAssignment> assigments, BuildContext buildContext)
        {
            var sortedAssigments = SortAssigmentsByDependencies(assigments.ToList());

            base.ApplyAssignments(instance, sortedAssigments, buildContext);
        }

        private IEnumerable<MemberAssignment> SortAssigmentsByDependencies(IList<MemberAssignment> propertyAssignments)
        {
            var dependencies = GetDependencies(propertyAssignments);
            var sortedAssigments = dependencies.SortDependencies();
            return sortedAssigments.Select(dependency => dependency.Assignment);
        }

        private IEnumerable<Dependency> GetDependencies(IList<MemberAssignment> propertyAssignments)
        {
            return propertyAssignments.Select(
                assignment =>
                {
                    var memberAssignments = propertyAssignments.Except(new[] {assignment});
                    return new Dependency(assignment, ObjectBuilderContext.MetadataProvider, memberAssignments);
                });
        }

        protected override object MakeCompatible(object instance, ConversionRequest conversionRequest, BuildContext buildContext)
        {
            var me = conversionRequest.Value as IMarkupExtension;
            object finalValue= conversionRequest.Value;
            if (me != null)
            {
                var value = me.GetValue(contextFactory.CreateExtensionContext(new Assignment(new KeyedInstance(instance), conversionRequest.Member, conversionRequest.Value), buildContext));
                finalValue = value;
            }

            return base.MakeCompatible(instance, new ConversionRequest(conversionRequest.Member, finalValue), buildContext);
        }

        protected override object CreateChildProperty(object parent, Member member, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            var metadata = ObjectBuilderContext.MetadataProvider.Get(parent.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && parent.GetType() == fragmentLoaderInfo.Type && member.MemberName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(nodeToBeCreated, this, buildContext);
            }
            else
            {
                return base.CreateChildProperty(parent, member, nodeToBeCreated, buildContext);
            }
        }       
    }
}