namespace SampleWpfApp
{
    using System.Collections.Generic;
    using OmniXaml.Services.Mvvm;

    // ReSharper disable once UnusedMember.Global
    public class MainWindowViewModel : ViewModel
    {
        private PersonViewModel selectedContact;

        public MainWindowViewModel(IPeopleService peopleService)
        {
            People = peopleService.GetPeople();

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