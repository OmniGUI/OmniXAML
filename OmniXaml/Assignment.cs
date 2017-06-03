namespace OmniXaml
{
    public class Assignment
    {
        public Assignment(KeyedInstance target, Member member, object value)
        {
            Target = target;
            Value = value;
            Member = member;
        }

        public Assignment(object target, Member member, object value) : this(new KeyedInstance(target), member, value)
        {           
        }

        public KeyedInstance Target { get; }

        public object Value { get; }

        public Member Member { get; }

        public void ExecuteAssignment()
        {            
            Member.SetValue(Target.Instance, Value);
        }

        public override string ToString()
        {
            return $"Assigment of {Value} to {Member} in a {Target}";
        }
    }
}