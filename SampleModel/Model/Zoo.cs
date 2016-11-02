namespace SampleModel.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using OmniXaml.DefaultLoader;

    [Namescope]
    public class Zoo
    {
        readonly IDictionary<string, Animal> animalNames = new Dictionary<string, Animal>();

        public Zoo()
        {
            Animals = new Collection<Animal>();
        }

        [Content]
        public ICollection<Animal> Animals { get; set; }

        public override string ToString()
        {
            var animalStrings = Animals.Select(animal => animal.ToString() + "\n") ;
            return "Zoo with the following animals: \n" + string.Concat(animalStrings);
        }   
    }
}