namespace SampleWpfApp
{
    using System;
    using OmniXaml.Services.Mvvm;

    public class PersonViewModel : ViewModel
    {
        public PersonViewModel(string name, string surname, Uri pictureUri)
        {
            Picture = pictureUri;
            Name = name;
            Surname = surname;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public Uri Picture { get; set; }
    }
}