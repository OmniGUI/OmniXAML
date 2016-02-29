namespace BasicModel.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using OmniXaml;
    using OmniXaml.Attributes;

    [ContentProperty("Animals")]
    // ReSharper disable once UnusedMember.Global
    public class Zoo : INameScope
    {
        readonly IDictionary<string, Animal> animalNames = new Dictionary<string, Animal>();

        public Zoo()
        {
            Animals = new Collection<Animal>();
        }

        public ICollection<Animal> Animals { get; set; }

        public override string ToString()
        {
            var animalStrings = Animals.Select(animal => animal.ToString() + "\n") ;
            return "Zoo with the following animals: \n" + string.Concat(animalStrings);
        }

        public void Register(string name, object scopedElement)
        {
            animalNames.Add(name, (Animal) scopedElement);
        }

        public object Find(string name)
        {
            return animalNames[name];
        }

        public void Unregister(string name)
        {
            animalNames.Remove(name);
        }
    }
}