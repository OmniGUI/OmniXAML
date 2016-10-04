namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ObjectBuilder
    {
        private readonly IInstanceCreator creator;

        public ObjectBuilder(IInstanceCreator creator)
        {
            this.creator = creator;
        }

        public object Create(ContructionNode node)
        {
            var instance = creator.Create(node.InstanceType);

            ApplyAssignments(instance, node.Assignments);
            
            return instance;
        }

        private static void ApplyAssignments(object instance, IEnumerable<PropertyAssignment> propertyAssignments)
        {
            foreach (var propertyAssignment in propertyAssignments)
            {
                var type = instance.GetType();
                var propertyInfo = type.GetRuntimeProperties().First(info => info.Name == propertyAssignment.Property.Name);
                propertyInfo.SetValue(instance, propertyAssignment.SourceValue);
            }
        }
    }
}