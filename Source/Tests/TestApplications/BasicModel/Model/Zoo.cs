namespace SampleOmniXAML.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using OmniXaml.Attributes;

    [ContentProperty("Animals")]
    public class Zoo
    {
        public Zoo()
        {
            Animals = new Collection<Animal>();
        }

        public ICollection<Animal> Animals { get; set; }

        public override string ToString()
        {
            var animalStrings = Animals.Select(animal => animal.ToString() + "\n") ;
            return "Zoom with the following animals: \n" + string.Concat(animalStrings);
        }
    }
}