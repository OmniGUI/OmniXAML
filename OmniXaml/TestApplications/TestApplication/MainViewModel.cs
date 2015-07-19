namespace TestApplication
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            People = new Collection<PersonViewModel>()
            {
                new PersonViewModel("Johnny", "Mnemonic"),
                new PersonViewModel("Inspector", "Gadget"),
            };
        }

        public ICollection<PersonViewModel> People { get; set; }
    }
}