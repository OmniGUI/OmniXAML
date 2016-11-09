namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;
    using Metadata;

    public class ExtendedObjectBuilder : ObjectBuilder
    {
        private readonly IContextFactory contextFactory;

        public ExtendedObjectBuilder(IInstanceCreator creator, ObjectBuilderContext objectBuilderContext, IContextFactory contextFactory)
            : base(creator, objectBuilderContext, contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        protected override void ApplyAssignments(object instance, IEnumerable<PropertyAssignment> propertyAssignments, BuildContext buildContext)
        {
            var sortedAssigments = SortAssigmentsByDependencies(propertyAssignments.ToList());

            base.ApplyAssignments(instance, sortedAssigments, buildContext);
        }

        private IEnumerable<PropertyAssignment> SortAssigmentsByDependencies(IList<PropertyAssignment> propertyAssignments)
        {
            var dependencies =
                propertyAssignments.Select(
                    assignment => new Dependency(assignment, ObjectBuilderContext.MetadataProvider, propertyAssignments.Except(new[] {assignment})));
            var sortedAssigments = dependencies.SortDependencies();
            return sortedAssigments.Select(dependency => dependency.Assignment);
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

        protected override object CreateChildProperty(object parent, Property property, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            var metadata = ObjectBuilderContext.MetadataProvider.Get(parent.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && parent.GetType() == fragmentLoaderInfo.Type && property.PropertyName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(nodeToBeCreated, this, buildContext);
            }
            else
            {
                return base.CreateChildProperty(parent, property, nodeToBeCreated, buildContext);
            }
        }       
    }

    public class Dependency : IDependency<Dependency>
    {
        public PropertyAssignment Assignment { get; }

        public Dependency(PropertyAssignment assignment, IMetadataProvider metadataProvider, IEnumerable<PropertyAssignment> propertyAssignments)
        {
            this.Assignment = assignment;
            Dependencies = GetDependencies(assignment, metadataProvider, propertyAssignments);
        }

        private IEnumerable<Dependency> GetDependencies(PropertyAssignment assignment, IMetadataProvider metadataProvider, IEnumerable<PropertyAssignment> propertyAssignments)
        {
            var dependencyRegistrations = metadataProvider.Get(assignment.Property.Owner).PropertyDependencies;

            if (dependencyRegistrations == null)
            {
                return new List<Dependency>();
            }            

            var registrations = dependencyRegistrations.Where(registration => registration.DependsOn == assignment.Property.PropertyName).ToList();

            var dependencies = registrations.Select(registration => propertyAssignments.First(propertyAssignment => registration.PropertyName == propertyAssignment.Property.PropertyName));

            return dependencies.Select(propertyAssignment => new Dependency(propertyAssignment, metadataProvider, propertyAssignments));
        }

        protected bool Equals(Dependency other)
        {
            return Equals(Assignment, other.Assignment);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Dependency) obj);
        }

        public override int GetHashCode()
        {
            return (Assignment != null ? Assignment.GetHashCode() : 0);
        }

        public IEnumerable<Dependency> Dependencies { get; }
    }
}