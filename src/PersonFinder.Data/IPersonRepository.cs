using PersonFinder.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonFinder.Data
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonById(int id);
        Task<IEnumerable<Person>> SearchPersonByName(string searchString);
        Task<Person> SaveNewPerson(Person person);
    }
}
