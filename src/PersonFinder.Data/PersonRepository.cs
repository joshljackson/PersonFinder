using PersonFinder.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PersonFinder.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace PersonFinder.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext _personContext;
        public PersonRepository(PersonContext personContext)
        {
            this._personContext = personContext;
        }

        public async Task<Person> GetPersonById(int id)
        {
            var personQuery = from p in _personContext.People
                             where p.Id == id
                             select p;

            var person = await personQuery.FirstOrDefaultAsync();

            return person;
        }

        public async Task<IEnumerable<Person>> SearchPersonByName(string searchString)
        {
            List<Person> peopleSearchResults = new List<Person>();

            if (!String.IsNullOrEmpty(searchString))
            {
                var loweredSearchString = searchString.ToLower();

                var personQuery = from p in this._personContext.People
                                  where p.Name.ToLower().Contains(loweredSearchString)
                                  select p;

                peopleSearchResults = await personQuery.ToListAsync();
            }

            return peopleSearchResults;
        }

        public async Task<Person> SaveNewPerson(Person person)
        {
            if (String.IsNullOrEmpty(person.Name) || String.IsNullOrWhiteSpace(person.Name))
                throw new ArgumentException($"A {nameof(Person.Name)} is required in order to save a {nameof(Person)} record");

            await this._personContext.AddAsync(person);
            await this._personContext.SaveChangesAsync();
            return person;
        }
    }
}
