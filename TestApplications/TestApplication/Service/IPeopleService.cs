namespace SampleWpfApp
{
    using System.Collections.Generic;

    public interface IPeopleService
    {
        ICollection<PersonViewModel> GetPeople();
    }
}