using Microsoft.EntityFrameworkCore;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.UnitOfWork.Repository;
using UniversityStudyPlatform.Models;

namespace Tests2
{
    [TestFixture]
    public class StudentRepositoryTests
    {
        private ApplicationDbContext _context;
        private StudentRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted(); // очищення перед кожним тестом
            _context.Database.EnsureCreated();

            _repository = new StudentRepository(_context);
        }

        [Test]
        public void GetStudentByPersonId_ReturnsCorrectStudent()
        {
            var person = new Person { Id = 1, Name = "Ivan", Surname = "Petrenko" };
            var student = new Student { Id = 1, PersonId = 1, Person = person };

            _context.Persons.Add(person);
            _context.Students.Add(student);
            _context.SaveChanges();

            var result = _repository.GetStudentByPersonId(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.PersonId);
            Assert.AreEqual("Ivan", result.Person.Name);
        }

        [Test]
        public void GetStudentPerformanceByStudentId_ReturnsPerformanceList()
        {
            var subject = new Subject { Id = 1, Name = "Math" };
            var student = new Student { Id = 1, PersonId = 1 };
            var accountBook = new AccountBook { Id = 1, StudentId = 1 };
            var perf1 = new StudentPerfomance { Id = 1, AccountBook = accountBook, Subject = subject, TotalPoint = 90 };
            var perf2 = new StudentPerfomance { Id = 2, AccountBook = accountBook, Subject = subject, TotalPoint = 80 };

            _context.Students.Add(student);
            _context.Subjects.Add(subject);
            _context.AccountBooks.Add(accountBook);
            _context.StudentPerfomances.AddRange(perf1, perf2);
            _context.SaveChanges();

            var results = _repository.GetStudentPerformanceByStudentId(1).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(90, results[0].TotalPoint);
            Assert.AreEqual("Math", results[0].Subject.Name);
        }

        [Test]
        public void CalculateStudentTotalGPA_ReturnsCorrectAverage()
        {
            var student = new Student { Id = 1, PersonId = 1 };
            var subject = new Subject { Id = 1, Name = "Physics" };
            var accountBook = new AccountBook { Id = 1, StudentId = 1 };
            var perf1 = new StudentPerfomance { Id = 1, AccountBook = accountBook, Subject = subject, TotalPoint = 85 };
            var perf2 = new StudentPerfomance { Id = 2, AccountBook = accountBook, Subject = subject, TotalPoint = 95 };

            _context.Students.Add(student);
            _context.Subjects.Add(subject);
            _context.AccountBooks.Add(accountBook);
            _context.StudentPerfomances.AddRange(perf1, perf2);
            _context.SaveChanges();

            var gpa = _repository.CalculateStudentTotalGPA(1);

            Assert.AreEqual(90, gpa);
        }

        [Test]
        public void CalculateStudentTotalGPA_ReturnsZero_WhenNoPerformance()
        {
            var student = new Student { Id = 1, PersonId = 1 };
            _context.Students.Add(student);
            _context.SaveChanges();

            var gpa = _repository.CalculateStudentTotalGPA(1);

            Assert.AreEqual(0, gpa);
        }
    }
}
