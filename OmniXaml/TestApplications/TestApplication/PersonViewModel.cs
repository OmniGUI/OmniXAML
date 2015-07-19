namespace TestApplication
{
    public class PersonViewModel : ViewModel
    {
        public string Name { get; }
        public string Surname { get; }

        public PersonViewModel(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }     
    }
}