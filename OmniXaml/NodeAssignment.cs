namespace OmniXaml
{
    public class NodeAssignment
    {
        public NodeAssignment(MemberAssignment assignment, object instance)
        {
            Assignment = assignment;
            Instance = instance;
        }

        public MemberAssignment Assignment { get; }
        public object Instance { get; }
    }
}