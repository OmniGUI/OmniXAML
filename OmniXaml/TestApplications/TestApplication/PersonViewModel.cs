namespace TestApplication
{
    public class PersonViewModel : ViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public PersonViewModel(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }     
    }
}