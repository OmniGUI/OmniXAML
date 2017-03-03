namespace OmniXaml
{
    public class Assignment
    {
        public Assignment(KeyedInstance target, Member property, object value)
        {
            Target = target;
            Value = value;
            Member = property;
        }

        public KeyedInstance Target { get; }

        public object Value { get; }

        public Member Member { get; }

        public void ExecuteAssignment()
        {            
            Member.SetValue(Target.Instance, Value);
        }
    }
}