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
    public class SheduleRepositoryTests
    {
        private ApplicationDbContext _context;
        private SheduleRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new SheduleRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // --- Edge case: student with no schedule ---
        [Test]
        public void GetAllShedule_ReturnsEmpty_WhenNoScheduleExistsForStudent()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var accountBook = new AccountBook { Student = student, GroupId = 1 };

            _context.Students.Add(student);
            _context.AccountBooks.Add(accountBook);
            _context.SaveChanges();

            var result = _repository.GetAllShedule(student);

            Assert.NotNull(result);
            Assert.AreEqual(Enum.GetNames(typeof(Day)).Length, result.Count);
            foreach (var day in result)
            {
                Assert.AreEqual(5, day.Count); // Each day should have 5 periods
                Assert.IsTrue(day.All(s => s == null)); // All periods should be null
            }
        }

        // --- Valid schedule data for student ---
        [Test]
        public void GetAllShedule_ReturnsSchedule_WhenScheduleExistsForStudent()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var accountBook = new AccountBook { Student = student, GroupId = 1 };

            _context.Students.Add(student);
            _context.AccountBooks.Add(accountBook);
            _context.SaveChanges();

            var subject = new Subject { Name = "Mathematics" };
            var teacher = new Teacher { PersonId = 1 };
            var sheduleEntry = new Shedule
            {
                Day = Day.MONDAY,
                NumberPara = 1,
                Teacher = teacher,
                GroupId = accountBook.GroupId,
                Subject = subject,
                Faculty = "Applied Mathematics"
            };

            _context.Shedule.Add(sheduleEntry);
            _context.SaveChanges();

            var result = _repository.GetAllShedule(student);

            Assert.NotNull(result);
            Assert.AreEqual(Enum.GetNames(typeof(Day)).Length, result.Count);
            var mondaySchedule = result[(int)Day.MONDAY];
            Assert.AreEqual(5, mondaySchedule.Count); // Monday should have 5 periods
            Assert.IsNotNull(mondaySchedule[0]); // First period should have a schedule entry
            Assert.AreEqual(subject.Name, mondaySchedule[0].Subject.Name);
        }

        // --- Edge case: student in a group with no schedules ---
        [Test]
        public void GetAllShedule_ReturnsNullPeriods_WhenNoScheduleExistsForGroup()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var accountBook = new AccountBook { Student = student, GroupId = 1 };

            _context.Students.Add(student);
            _context.AccountBooks.Add(accountBook);
            _context.SaveChanges();

            var result = _repository.GetAllShedule(student);

            Assert.NotNull(result);
            foreach (var day in result)
            {
                Assert.AreEqual(5, day.Count); // Each day should have 5 periods
                Assert.IsTrue(day.All(s => s == null)); // All periods should be null
            }
        }

        // --- Edge case: schedule data with missing periods ---
        [Test]
        public void GetAllShedule_ReturnsSomeNullPeriods_WhenSomePeriodsAreMissing()
        {
            var student = new Student { Id = 1, PersonId = 123 };
            var accountBook = new AccountBook { Student = student, GroupId = 1 };

            _context.Students.Add(student);
            _context.AccountBooks.Add(accountBook);
            _context.SaveChanges();

            var subject = new Subject { Name = "Mathematics" };
            var teacher = new Teacher { PersonId = 1,};
            var sheduleEntry = new Shedule
            {
                Day = Day.MONDAY,
                NumberPara = 1,
                Teacher = teacher,
                GroupId = accountBook.GroupId,
                Subject = subject,
                Faculty = "Applied Mathematics"
            };

            _context.Shedule.Add(sheduleEntry);
            _context.SaveChanges();

            var result = _repository.GetAllShedule(student);

            Assert.NotNull(result);
            var mondaySchedule = result[(int)Day.MONDAY];
            Assert.AreEqual(5, mondaySchedule.Count); // Monday should have 5 periods
            Assert.IsNotNull(mondaySchedule[0]); // First period should have a schedule entry
            Assert.AreEqual(subject.Name, mondaySchedule[0].Subject.Name);
            Assert.IsNull(mondaySchedule[1]); // The second period should be null as it's missing
        }
    }
}
