namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;
    using Metadata;

    public class Dependency : IDependency<Dependency>
    {
        public MemberAssignment Assignment { get; }

        public Dependency(MemberAssignment assignment, IMetadataProvider metadataProvider, IEnumerable<MemberAssignment> propertyAssignments)
        {
            this.Assignment = assignment;
            Dependencies = GetDependencies(assignment, metadataProvider, propertyAssignments).ToList();
        }

        private IEnumerable<Dependency> GetDependencies(MemberAssignment assignment, IMetadataProvider metadataProvider, IEnumerable<MemberAssignment> propertyAssignments)
        {
            var dependencyRegistrations = metadataProvider.Get(assignment.Member.Owner).PropertyDependencies;

            if (dependencyRegistrations == null)
            {
                return new List<Dependency>();
            }            

            var registrations = dependencyRegistrations.Where(registration => registration.PropertyName == assignment.Member.MemberName).ToList();

            var dependencies = registrations.Select(registration => propertyAssignments.First(propertyAssignment => registration.DependsOn == propertyAssignment.Member.MemberName));

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
            return Assignment?.GetHashCode() ?? 0;
        }

        public IEnumerable<Dependency> Dependencies { get; }
    }
}