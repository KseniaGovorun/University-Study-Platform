using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.UnitOfWork.Repository;
using UniversityStudyPlatform.Models;

namespace Tests2
{
    [TestFixture]
    public class PersonRepositoryTests
    {
        private ApplicationDbContext _context;
        private PersonRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new PersonRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // --- Test for getting a person by name ---
        [Test]
        public void GetPersonByName_ReturnsCorrectPerson_WhenPersonExists()
        {
            var person = new Person { Name = "John", Surname = "Doe" };
            _context.Persons.Add(person);
            _context.SaveChanges();

            var result = _repository.GetPersonByName("John");

            Assert.NotNull(result);
            Assert.AreEqual("John", result.Name);
        }

        // --- Test for getting a person by name that does not exist ---
        [Test]
        public void GetPersonByName_ReturnsNull_WhenPersonDoesNotExist()
        {
            var result = _repository.GetPersonByName("Nonexistent");

            Assert.IsNull(result);
        }

        // --- Test for getting a person by surname ---
        [Test]
        public void GetPersonBySurname_ReturnsCorrectPerson_WhenPersonExists()
        {
            var person = new Person { Name = "Jane", Surname = "Smith" };
            _context.Persons.Add(person);
            _context.SaveChanges();

            var result = _repository.GetPersonBySurname("Smith");

            Assert.NotNull(result);
            Assert.AreEqual("Smith", result.Surname);
        }

        // --- Test for getting a person by surname that does not exist ---
        [Test]
        public void GetPersonBySurname_ReturnsNull_WhenPersonDoesNotExist()
        {
            var result = _repository.GetPersonBySurname("Nonexistent");

            Assert.IsNull(result);
        }

        // --- Test for checking if person exists by LoginData ---
        [Test]
        public void PersonExistsByLoginData_ReturnsTrue_WhenPersonExists()
        {
            var loginData = new LoginData { Email = "user1@gmail.com", Password = "password123" };
            var person = new Person { Name = "John", Surname = "Doe", LoginData = loginData };
            _context.Persons.Add(person);
            _context.SaveChanges();

            var result = _repository.PersonExistsByLoginData(loginData);

            Assert.IsTrue(result);
        }

        // --- Test for checking if person exists by LoginData when no person exists ---
        [Test]
        public void PersonExistsByLoginData_ReturnsFalse_WhenPersonDoesNotExist()
        {
            var loginData = new LoginData { Email = "user1@gmail.com", Password = "password123" };

            var result = _repository.PersonExistsByLoginData(loginData);

            Assert.IsFalse(result);
        }

        // --- Test for updating a person's details ---
        [Test]
        public void UpdatePerson_UpdatesPersonDetails_WhenPersonExists()
        {
            var person = new Person { Name = "John", Surname = "Doe" };
            _context.Persons.Add(person);
            _context.SaveChanges();

            person.Name = "Johnathan"; // Change name
            _repository.UpdatePerson(person);

            var updatedPerson = _repository.GetPersonByName("Johnathan");

            Assert.NotNull(updatedPerson);
            Assert.AreEqual("Johnathan", updatedPerson.Name);
        }

        // --- Test for getting persons by surname ---
        [Test]
        public void GetPersonsBySurname_ReturnsListOfPersons_WhenMultiplePersonsHaveSameSurname()
        {
            var person1 = new Person { Name = "John", Surname = "Smith" };
            var person2 = new Person { Name = "Jane", Surname = "Smith" };
            _context.Persons.AddRange(person1, person2);
            _context.SaveChanges();

            var result = _repository.GetPersonsBySurname("Smith");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Smith", result[0].Surname);
            Assert.AreEqual("Smith", result[1].Surname);
        }

        // --- Test for getting persons by surname when no one has that surname ---
        [Test]
        public void GetPersonsBySurname_ReturnsEmptyList_WhenNoPersonsHaveThatSurname()
        {
            var result = _repository.GetPersonsBySurname("Nonexistent");

            Assert.AreEqual(0, result.Count);
        }
    }

}
