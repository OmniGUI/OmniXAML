namespace OmniXaml.Ambient
{
    using System.Collections.Generic;
    using Glass.Core.Glass.Core;

    public class AmbientRegistrator : IAmbientRegistrator
    {
        private readonly StackingLinkedList<AmbientPropertyAssignment> propertyAssignments = new StackingLinkedList<AmbientPropertyAssignment>();

        public void RegisterAssignment(AmbientPropertyAssignment assignment)
        {
            propertyAssignments.Push(assignment);
        }

        public IEnumerable<AmbientPropertyAssignment> Assigments => propertyAssignments.ToList();
    }
}