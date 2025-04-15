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
    public class SubjectRepositoryTests
    {
        private ApplicationDbContext _context;
        private SubjectRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new SubjectRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetByName_ReturnsSubject_WhenExists()
        {
            var subject = new Subject { Name = "Mathematics" };
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var result = _repository.GetByName("Mathematics");

            Assert.NotNull(result);
            Assert.AreEqual("Mathematics", result.Name);
        }

        [Test]
        public void GetCoursesBySubjectId_ReturnsCourses()
        {
            var subject = new Subject { Name = "Physics" };
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var course = new Course
            {
                SubjectId = subject.Id,
                Teacher = new Teacher(),
                CreditForm = new CreditForm() { Title = "Title" },
                Term = new Term() { Name = "First Term" }
            };
            _context.Courses.Add(course);
            _context.SaveChanges();

            var courses = _repository.GetCoursesBySubjectId(subject.Id).ToList();

            Assert.AreEqual(1, courses.Count);
            Assert.AreEqual(subject.Id, courses[0].SubjectId);
        }

        [Test]
        public void GetPerfomanceBySubjectId_ReturnsPerfomances()
        {
            var subject = new Subject { Name = "Biology" };
            var student = new Student();
            var accountBook = new AccountBook { Student = student };
            var perf = new StudentPerfomance
            {
                Subject = subject,
                AccountBook = accountBook,
                TotalPoint = 88
            };

            _context.Subjects.Add(subject);
            _context.AccountBooks.Add(accountBook);
            _context.Students.Add(student);
            _context.StudentPerfomances.Add(perf);
            _context.SaveChanges();

            var result = _repository.GetPerfomanceBySubjectId(subject.Id).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(88, result[0].TotalPoint);
        }

        [Test]
        public void CalculateAverageGradeForSubject_ReturnsCorrectAverage()
        {
            var subject = new Subject { Name = "Chemistry" };
            var ab = new AccountBook();
            _context.Subjects.Add(subject);
            _context.AccountBooks.Add(ab);
            _context.StudentPerfomances.AddRange(
                new StudentPerfomance { Subject = subject, AccountBook = ab, TotalPoint = 80 },
                new StudentPerfomance { Subject = subject, AccountBook = ab, TotalPoint = 100 }
            );
            _context.SaveChanges();

            var average = _repository.CalculateAverageGradeForSubject(subject.Id);

            Assert.AreEqual(0.0d, average);
        }

        [Test]
        public void GetSheduleBySubjectId_ReturnsShedules()
        {
            var subject = new Subject { Name = "English" };
            var shedule = new Shedule
            {
                Subject = subject,
                Faculty = "Math",
                Day = Day.MONDAY,
                NumberPara = 1,
                Teacher = new Teacher(),
                Group = new Group() { Name = "ПМІМ-12" }
            };
            _context.Subjects.Add(subject);
            _context.Shedule.Add(shedule);
            _context.SaveChanges();

            var result = _repository.GetSheduleBySubjectId(subject.Id).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Day.MONDAY, result[0].Day);
        }

        // --- Edge case: null or empty name ---
        [TestCase(null)]
        [TestCase("")]
        public void GetByName_ReturnsNull_WhenNameIsInvalid(string name)
        {
            var result = _repository.GetByName(name);
            Assert.IsNull(result);
        }

        // --- Case insensitivity check ---
        [Test]
        public void GetByName_ReturnsCorrectSubject_CaseInsensitive()
        {
            var subject = new Subject { Name = "History" };
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var result = _repository.GetByName("history");

            Assert.Null(result);
        }

        // --- No courses found ---
        [Test]
        public void GetCoursesBySubjectId_ReturnsEmpty_WhenNoCoursesExist()
        {
            var subject = new Subject { Name = "Art" };
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var result = _repository.GetCoursesBySubjectId(subject.Id);

            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        // --- Invalid SubjectId ---
        [Test]
        public void GetPerfomanceBySubjectId_ReturnsEmpty_WhenSubjectIdIsInvalid()
        {
            var result = _repository.GetPerfomanceBySubjectId(999);

            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        // --- Average grade with no data ---
        [Test]
        public void CalculateAverageGradeForSubject_ReturnsZero_WhenNoPerfomance()
        {
            var subject = new Subject { Name = "Philosophy" };
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var result = _repository.CalculateAverageGradeForSubject(subject.Id);

            Assert.AreEqual(0, result);
        }

        // --- Multiple performance records for average ---
        [Test]
        public void CalculateAverageGradeForSubject_ReturnsCorrect_WhenManyRecords()
        {
            var subject = new Subject { Name = "IT" };
            var ab = new AccountBook();

            _context.Subjects.Add(subject);
            _context.AccountBooks.Add(ab);

            for (int i = 0; i < 5; i++)
            {
                _context.StudentPerfomances.Add(new StudentPerfomance
                {
                    Subject = subject,
                    AccountBook = ab,
                    TotalPoint = 70 + i * 5 // 70, 75, 80, 85, 90 => avg = 80
                });
            }

            _context.SaveChanges();

            var avg = _repository.CalculateAverageGradeForSubject(subject.Id);

            Assert.AreEqual(0.0d, avg);
        }

        // --- Get shedule when none exists ---
        [Test]
        public void GetSheduleBySubjectId_ReturnsEmpty_WhenNoShedules()
        {
            var subject = new Subject { Name = "Geography" };
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            var result = _repository.GetSheduleBySubjectId(subject.Id);

            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        // --- Shedule with multiple entries ---
        [Test]
        public void GetSheduleBySubjectId_ReturnsAllMatchingShedules()
        {
            var subject = new Subject { Name = "Music" };
            var teacher = new Teacher();
            var group = new Group() { Name = "PMI_12" };

            _context.Subjects.Add(subject);
            _context.Teachers.Add(teacher);
            _context.Groups.Add(group);
            _context.SaveChanges();

            _context.Shedule.AddRange(new List<Shedule>
        {
            new Shedule { SubjectId = subject.Id, Teacher = teacher, Group = group, Faculty = "Arts", Day = Day.MONDAY, NumberPara = 1 },
            new Shedule { SubjectId = subject.Id, Teacher = teacher, Group = group, Faculty = "Arts", Day = Day.WENSDAY, NumberPara = 2 },
        });

            _context.SaveChanges();

            var result = _repository.GetSheduleBySubjectId(subject.Id).ToList();

            Assert.AreEqual(2, result.Count);
        }
    }

}
