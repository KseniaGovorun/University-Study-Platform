using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.DataAccess.UnitOfWork.Repository;
using UniversityStudyPlatform.Models;

namespace Tests2
{
    [TestFixture]
    public class TeacherRepositoryTests
    {
        private ApplicationDbContext _context;
        private TeacherRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TeacherDbTest")
                .Options;

            _context = new ApplicationDbContext(options);

            _repository = new TeacherRepository(_context);
        }

        [Test]
        public void GetTeacherNameById_ShouldReturnFullName_WhenTeacherExists()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "Ivan", Surname = "Petrenko" };
            var teacher = new Teacher { Id = 10, PersonId = 1, Person = person };

            _context.Persons.Add(person);
            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            // Act
            var result = _repository.GetTeacherNameById(10);

            // Assert
            Assert.AreEqual("Ivan Petrenko", result);
        }

        [Test]
        public void GetTeacherNameById_ShouldReturnNull_WhenTeacherNotFound()
        {
            // Act
            var result = _repository.GetTeacherNameById(999); // неіснуючий ID

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetTeacherNameById_ShouldNotThrow_WhenPersonNotFound()
        {
            // Arrange
            var teacher = new Teacher { Id = 2, PersonId = 100 }; // Person не існує
            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            // Act
            var result = _repository.GetTeacherNameById(2);

            // Assert
            Assert.IsNull(result);
        }
    }
}
