namespace SampleOmniXAML.Model
{
    public class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Species: {Species}";
        }
    }
}