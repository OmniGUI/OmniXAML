namespace OmniXaml
{
    public class Assignment
    {
        public Assignment(object instance, Property property, object value)
        {
            Instance = instance;
            Value = value;
            Property = property;
        }

        public object Instance { get; }

        public object Value { get; }

        public Property Property { get; }

        public void ExecuteAssignment()
        {
            Property.SetValue(Instance, Value);
        }

        public Assignment ReplaceValue(object value)
        {
            return new Assignment(Instance, Property, value);
        }
    }
}