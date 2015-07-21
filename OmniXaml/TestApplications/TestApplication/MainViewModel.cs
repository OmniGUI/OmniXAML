namespace TestApplication
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class MainViewModel : ViewModel
    {
        private PersonViewModel selectedContact;

        public MainViewModel()
        {
            People = new Collection<PersonViewModel>()
            {
                new PersonViewModel("Johnny", "Mnemonic"),
                new PersonViewModel("Inspector", "Gadget"),
            };

            Title = "Title";
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

        public string SelectedName
        {
            get { return selectedContact?.Name; }
            set
            {
                selectedContact.Name = value;
                OnPropertyChanged();
            }
        }

        public string SelectedSurname
        {
            get { return selectedContact?.Surname; }
            set
            {
                selectedContact.Surname = value;
                OnPropertyChanged();
            }
        }

        public string Title { get; set; }
    }
}