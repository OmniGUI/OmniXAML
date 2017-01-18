using Glass.Core;

namespace OmniXaml.Ambient
{
    using System.Collections.Generic;

    public class AmbientRegistrator : IAmbientRegistrator
    {
        private readonly StackingLinkedList<AmbientMemberAssignment> propertyAssignments = new StackingLinkedList<AmbientMemberAssignment>();
        private readonly StackingLinkedList<object> instances = new StackingLinkedList<object>();

        public void RegisterAssignment(AmbientMemberAssignment assignment)
        {
            propertyAssignments.Push(assignment);
        }

        public void RegisterInstance(object instance)
        {
            instances.Push(instance);
        }

        public IEnumerable<AmbientMemberAssignment> Assigments => propertyAssignments.ToList();
        public IEnumerable<object> Instances => instances.ToList();
    }
}