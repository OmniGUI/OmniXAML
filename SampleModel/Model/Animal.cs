namespace SampleModel.Model
{
    using OmniXaml.DefaultLoader;

    public class Animal
    {
        [Name]
        public string Name { get; set; }
        public string Species { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Species: {Species}";
        }
    }
}