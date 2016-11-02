namespace OmniXaml
{
    public class Directive
    {
        public Directive(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}