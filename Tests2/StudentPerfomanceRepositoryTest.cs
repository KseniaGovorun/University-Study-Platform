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
    public class StudentPerfomanceRepositoryTests
    {
        private ApplicationDbContext _context;
        private StudentPerfomanceRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new StudentPerfomanceRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // --- Edge case: student with no performance records ---
        [Test]
        public void GetStudentPerfomanceByStudent_ReturnsEmpty_WhenNoPerfomanceExists()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            _context.Students.Add(student);
            _context.SaveChanges();

            var result = _repository.GetStudentPerfomanceByStudent(student);

            Assert.IsEmpty(result);
        }

        // --- Get student performance with valid data ---
        [Test]
        public void GetStudentPerfomanceByStudent_ReturnsPerfomances_WhenStudentHasPerfomances()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var accountBook = new AccountBook { Student = student };
            var subject = new Subject { Name = "Mathematics" };

            _context.Students.Add(student);
            _context.AccountBooks.Add(accountBook);
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var performance = new StudentPerfomance
            {
                AccountBookId = accountBook.Id,
                SubjectId = subject.Id,
                CurrentPoint = 80,
                ExamPoint = 90,
                TotalPoint = 170,
                SemesterNumber = 1
            };

            _context.StudentPerfomances.Add(performance);
            _context.SaveChanges();

            var result = _repository.GetStudentPerfomanceByStudent(student);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(170, result[0].TotalPoint);
            Assert.AreEqual(subject.Name, result[0].Subject.Name);
        }

        // --- Edge case: student with performance but no account book ---
        [Test]
        public void GetStudentPerfomanceByStudent_ReturnsEmpty_WhenStudentHasNoAccountBook()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var subject = new Subject { Name = "Physics" };

            _context.Students.Add(student);
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var performance = new StudentPerfomance
            {
                AccountBookId = 999, // Invalid AccountBookId
                SubjectId = subject.Id,
                CurrentPoint = 85,
                ExamPoint = 95,
                TotalPoint = 180,
                SemesterNumber = 1
            };

            _context.StudentPerfomances.Add(performance);
            _context.SaveChanges();

            var result = _repository.GetStudentPerfomanceByStudent(student);

            Assert.IsEmpty(result);
        }

        // --- Student performance retrieval with multiple subjects ---
        [Test]
        public void GetStudentPerfomanceByStudent_ReturnsMultiplePerfomances_WhenStudentHasMultipleSubjects()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var accountBook = new AccountBook { Student = student };
            var subject1 = new Subject { Name = "English" };
            var subject2 = new Subject { Name = "History" };

            _context.Students.Add(student);
            _context.AccountBooks.Add(accountBook);
            _context.Subjects.AddRange(subject1, subject2);
            _context.SaveChanges();

            var performance1 = new StudentPerfomance
            {
                AccountBookId = accountBook.Id,
                SubjectId = subject1.Id,
                CurrentPoint = 75,
                ExamPoint = 85,
                TotalPoint = 160,
                SemesterNumber = 1
            };

            var performance2 = new StudentPerfomance
            {
                AccountBookId = accountBook.Id,
                SubjectId = subject2.Id,
                CurrentPoint = 80,
                ExamPoint = 90,
                TotalPoint = 170,
                SemesterNumber = 1
            };

            _context.StudentPerfomances.AddRange(performance1, performance2);
            _context.SaveChanges();

            var result = _repository.GetStudentPerfomanceByStudent(student);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(170.0f, result[0].TotalPoint);
            Assert.AreEqual(160.0f, result[1].TotalPoint);
        }
    }
}
