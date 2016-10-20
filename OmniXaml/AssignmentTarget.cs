namespace OmniXaml
{
    public class AssignmentTarget
    {
        public AssignmentTarget(object instance, Property property, object value)
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

        public AssignmentTarget ChangeValue(object value)
        {
            return new AssignmentTarget(Instance, Property, value);
        }
    }
}