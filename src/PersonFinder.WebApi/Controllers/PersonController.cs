using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonFinder.Contracts;
using PersonFinder.Data;

namespace PersonFinder.WebApi.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonController(ILogger<PersonController> logger, IPersonRepository personRepository)
        {
            this._logger = logger;
            this._personRepository = personRepository;
        }

        /// <summary>
        /// Retreives a person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/person/{id}")]
        public async Task<ActionResult<Person>> GetPersonById(int id)
        {
            var person = await this._personRepository.GetPersonById(id);

            return this.Ok(person);
        }

        /// <summary>
        /// Searches for people using the passed in PersonSearchQuery object
        /// </summary>
        /// <param name="personSearchQuery">The object containing the search parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/person/search")]
        public async Task<ActionResult<IEnumerable<Person>>> SearchPerson([FromBody] PersonSearchQuery personSearchQuery)
        {
            if (personSearchQuery == null)
                throw new ArgumentNullException($"The passed in {nameof(PersonSearchQuery)} was null. A search object is required.");

            this._logger.LogDebug($"Searching for people with search string {personSearchQuery?.SearchString}");
            var searchResults = await this._personRepository.SearchPersonByName(personSearchQuery?.SearchString);

            return this.Ok(searchResults);
        }

        /// <summary>
        /// Saves a new Person record to the database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/person")]
        public async Task<ActionResult<Person>> SaveNewPerson([FromBody] Person person)
        {
            var savedPerson = await this._personRepository.SaveNewPerson(person);

            return this.Created($"api/person/{savedPerson.Id}", savedPerson);
        }
    }
}
