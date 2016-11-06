namespace OmniXaml
{
    public class Assignment
    {
        public Assignment(object instance, Member property, object value)
        {
            Instance = instance;
            Value = value;
            Member = property;
        }

        public object Instance { get; }

        public object Value { get; }

        public Member Member { get; }

        public void ExecuteAssignment()
        {
            Member.SetValue(Instance, Value);
        }

        public Assignment ReplaceValue(object value)
        {
            return new Assignment(Instance, Member, value);
        }
    }
}