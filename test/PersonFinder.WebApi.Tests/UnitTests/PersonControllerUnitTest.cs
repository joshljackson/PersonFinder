using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PersonFinder.Data;
using PersonFinder.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonFinder.WebApi.UnitTests
{
    [TestFixture]

    public class PersonControllerUnitTest
    {
        [Test]
        public async Task NullSearchObjectThrowsException()
        {
            //Assemble
            using var controller = new PersonController(
                new Mock<ILogger<PersonController>>().Object,
                new Mock<IPersonRepository>().Object
                );

            //Act
            try
            {
                var searchResults = await controller.SearchPerson(null);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ArgumentNullException))
                    Assert.Pass();
            }
            Assert.Fail($"A null search object should throw a {nameof(ArgumentNullException)}");
        }
    }
}
