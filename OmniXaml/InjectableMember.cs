namespace OmniXaml
{
    public class InjectableMember
    {
        public InjectableMember(object value)
        {
            Value = value;
        }

        public object Value { get; set; }
        public string Name { get; set; }
    }
}