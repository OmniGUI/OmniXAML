namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly IContextFactory contextFactory;

        public ExtendedObjectBuilder(IInstanceCreator creator, ObjectBuilderContext objectBuilderContext, IContextFactory contextFactory)
            : base(creator, objectBuilderContext, contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        protected override void ApplyAssignments(object instance, IEnumerable<MemberAssignment> propertyAssignments, BuildContext buildContext)
        {
            var sortedAssigments = SortAssigmentsByDependencies(propertyAssignments.ToList());

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

        protected override Assignment ToCompatibleValue(Assignment assignment, BuildContext buildContext)
        {
            var me = assignment.Value as IMarkupExtension;
            if (me != null)
            {
                var value = me.GetValue(contextFactory.CreateExtensionContext(assignment, buildContext));
                assignment = assignment.ReplaceValue(value);
            }

            return base.ToCompatibleValue(assignment, buildContext);
        }

        protected override object CreateChildProperty(object parent, Member property, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            var metadata = ObjectBuilderContext.MetadataProvider.Get(parent.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && parent.GetType() == fragmentLoaderInfo.Type && property.MemberName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(nodeToBeCreated, this, buildContext);
            }
            else
            {
                return base.CreateChildProperty(parent, property, nodeToBeCreated, buildContext);
            }
        }       
    }
}