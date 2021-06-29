using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonFinder.Data.DataContext;
using PersonFinder.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


// EC
    // powershell or bash scripts to search or add?
    // latency in the api call?
    // add a command line interface?


namespace PersonFinder.Data.RepositoryTests
{
    [TestFixture]
    public class PersonRepositoryUnitTest
    {
        private readonly DbContextOptions<PersonContext> ContextOptions;
        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        public PersonRepositoryUnitTest()
        {
            var options = new DbContextOptionsBuilder<PersonContext>().UseInMemoryDatabase("PersonDatabase").Options;
            ContextOptions = options;
            this._services = new ServiceCollection();
            this.ConfigureServices();
        }

        [SetUp]
        public void SetupTestData()
        {
            using var context = new PersonContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //var people = new List<Person>() {
            //    new Person(1, "Josh", 32, null, "cycling, languages, starwars"),
            //    new Person(2, "Joshua", 34, null, "running"),
            //    new Person(3, "Andrew", 33, null, "cycling"),
            //    new Person(4, "Zach", 33, null, "fishing, aviation")
            //};

            var people = new List<Person>() {
                new Person(1, "Anakin", 20, null, "Pod racing, combat, Senators from Naboo"),
                new Person(2, "Zam Wesell", 32, null, "Bounty Hunting"),
                new Person(3, "Captain Rex", 8, "Kamino", "Combat"),
                new Person(4, "Wat Tambor", 50, null, "Techno Union, Separatist movement"),
                new Person(5, "Ahsoka Tano", 13, null, "Lightsaber")
            };

            context.AddRange(people);
            context.SaveChanges();
        }


        #region GetPersonById

        [Test]
        public async Task CanGetPersonById()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();

            //Act
            var person = await repository.GetPersonById(4);

            //Assert
            Assert.That(person, Is.Not.Null, $"No person record was returned");
            Assert.That(person.Id, Is.EqualTo(4), $"The returned person record had an Id of {person.Id}, was expecting 4");
        }

        [Test]
        public async Task IdNotPresentReturnsNull()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();

            //Act
            var person = await repository.GetPersonById(99);

            //Assert
            Assert.That(person, Is.Null, $"No person record should have been returned");
        }

        #endregion

        #region SearchPersonByName

        [Test]
        public async Task SearchByNamePresentReturnsValue()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var searchString = "Ahsoka Tano";

            //Act
            var peopleList = await repository.SearchPersonByName(searchString);

            //Assert
            Assert.That(peopleList, Is.Not.Null, $"A null list was returned");
            Assert.That(peopleList, Is.Not.Empty, $"No person record was returned");
            Assert.That(peopleList.Count(), Is.EqualTo(1), $"Only one matching record was expected");
            Assert.That(peopleList.First().Name.ToLower().Contains(searchString.ToLower()));
        }

        [Test]
        public async Task SearchByNameCaseInsensitive()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var searchString = "aHsOKa tanO";

            //Act
            var peopleList = await repository.SearchPersonByName(searchString);

            //Assert
            Assert.That(peopleList, Is.Not.Null, $"A null list was returned");
            Assert.That(peopleList, Is.Not.Empty, $"No person record was returned");
            Assert.That(peopleList.Count(), Is.EqualTo(1), $"Only one matching record was expected");
            Assert.That(peopleList.First().Name.ToLower().Contains(searchString.ToLower()));
        }

        [Test]
        public async Task SearchByNameNotPresentReturnsEmpty()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var searchString = "This is not an actual name";

            //Act
            var peopleList = await repository.SearchPersonByName(searchString);

            //Assert
            Assert.That(peopleList, Is.Not.Null, $"A null list was returned");
            Assert.That(peopleList, Is.Empty, $"There should not have been a match so the list should be empty");
        }

        [Test]
        public async Task SearchByNameSubstringReturnsAllMatches()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var searchString = "am";

            //Act
            var peopleList = await repository.SearchPersonByName(searchString);

            //Assert
            Assert.That(peopleList, Is.Not.Null, $"A null list was returned");
            Assert.That(peopleList, Is.Not.Empty, $"No person record was returned");
            Assert.That(peopleList.Count(), Is.EqualTo(2), $"All matching records should be returned.");
        }

        [Test]
        public async Task SearchByNameEmptySearchStringReturnsEmptyList()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var searchString = "";

            //Act
            var peopleList = await repository.SearchPersonByName(searchString);

            //Assert
            Assert.That(peopleList, Is.Not.Null, $"A null list was returned");
            Assert.That(peopleList, Is.Empty, $"The list should have been empty");
        }

        #endregion

        #region AddPerson

        [Test]
        public async Task NewPersonCanBeAdded()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var context = this._serviceProvider.GetService<PersonContext>();
            var newPerson = new Person()
            {
                Name = "Cad Bane",
                Address = "Doros",
                Age = 45,
                Interests = "Bounty Hunting"
            };

            //Act
            var savedPerson = await repository.SaveNewPerson(newPerson);

            //Assert
            Assert.That(savedPerson, Is.Not.Null, $"The save method should return the person that was saved");
            Assert.That(savedPerson.Id, Is.Not.Zero, $"An Id should have been assigned");
            var fetchedPerson = context.People.Where(p => p.Id == 6).FirstOrDefault();
            Assert.That(fetchedPerson, Is.Not.Null, $"The person should be able to be accessed from the db context after the repository save method returns.");
            Assert.That(fetchedPerson.Name, Is.EqualTo("Cad Bane"), $"The value of {nameof(Person.Name)} was not correct");
            Assert.That(fetchedPerson.Address, Is.EqualTo("Doros"), $"The value of {nameof(Person.Address)} was not correct");
            Assert.That(fetchedPerson.Age, Is.EqualTo(45), $"The value of {nameof(Person.Age)} was not correct");
            Assert.That(fetchedPerson.Interests, Is.EqualTo("Bounty Hunting"), $"The value of {nameof(Person.Interests)} was not correct");
        }

        [Test]
        public async Task SavingPersonWithEmptyNameThrowsException()
        {
            //Assemble
            var repository = this._serviceProvider.GetService<IPersonRepository>();
            var newPerson = new Person()
            {
                Name = "",
                Address = "none",
                Age = 1
            };

            //Act
            try
            {
                var savedPerson = await repository.SaveNewPerson(newPerson);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ArgumentException))
                    Assert.Pass();
            }
            Assert.Fail($"Attempting to save a person with no name should throw an exception of type {nameof(ArgumentException)}");
        }

        #endregion


        private void ConfigureServices()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configurationRoot = configurationBuilder.Build();
            this.ConfigureServices(this._services, configurationRoot);
            this._serviceProvider = this._services.BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PersonContext>(o => o.UseInMemoryDatabase("PersonDatabase"));
            services.AddTransient<IPersonRepository, PersonRepository>();
        }
    }
}
