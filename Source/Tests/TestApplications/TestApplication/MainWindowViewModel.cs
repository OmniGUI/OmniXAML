namespace TestApplication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OmniXaml.AppServices.Mvvm;

    public class MainWindowViewModel : ViewModel
    {
        private PersonViewModel selectedContact;

        public MainWindowViewModel()
        {
            People = new Collection<PersonViewModel>()
            {
                new PersonViewModel("Johnny", "Mnemonic", new Uri("Images/johnny_mnemonic.jpg", UriKind.Relative)),
                new PersonViewModel("Inspector", "Gadget", new Uri("Images/inspector_gadget.png", UriKind.Relative)),
                new PersonViewModel("Tom", "Jones", new Uri("Images/tom_jones.jpg", UriKind.Relative)),
                new PersonViewModel("Soap", "MacTavish", new Uri("Images/soap_mactavish.png", UriKind.Relative)),
                new PersonViewModel("Pino", "D'Angio", new Uri("Images/pino_dangio.jpg", UriKind.Relative))
            };

            Title = "Main Window";
        }

        public ICollection<PersonViewModel> People { get; set; }

        public PersonViewModel SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value; 
                OnPropertyChanged();        
            }
        }
        
        public string Title { get; set; }
    }
}