namespace OmniXaml.Tests.Classes
{
    using Attributes;

    public class Setter
    {
        public string Property { get; set; }

        [DependsOn("Property")]
        public string Value { get; set; }
    }
}