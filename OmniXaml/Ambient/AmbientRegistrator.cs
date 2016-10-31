namespace OmniXaml.Ambient
{
    using System.Collections.Generic;
    using Glass.Core.Glass.Core;

    public class AmbientRegistrator : IAmbientRegistrator
    {
        private readonly StackingLinkedList<AmbientPropertyAssignment> propertyAssignments = new StackingLinkedList<AmbientPropertyAssignment>();
        private readonly StackingLinkedList<object> instances = new StackingLinkedList<object>();

        public void RegisterAssignment(AmbientPropertyAssignment assignment)
        {
            propertyAssignments.Push(assignment);
        }

        public void RegisterInstance(object instance)
        {
            instances.Push(instance);
        }

        public IEnumerable<AmbientPropertyAssignment> Assigments => propertyAssignments.ToList();
        public IEnumerable<object> Instances => instances.ToList();
    }
}