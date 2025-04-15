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
    public class AssignmentRepositoryTests
    {
        private ApplicationDbContext _context;
        private AssignmentRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Arrange test data
            var student = new Student { Id = 1 };
            var teacher = new Teacher { Id = 1 };
            var group = new Group { Id = 1, Name = "PMI-12" };
            var course = new Course { Id = 1 };
            var courseGroup = new CourseGroup { Id = 1, CourseId = 1, GroupId = 1, Title = "CourseGroup1", PhotoUrl = "" };
            var accountBook = new AccountBook { Id = 1, StudentId = student.Id, GroupId = group.Id };

            var assignment1 = new Assignment { Id = 1, Description = "Assignment 1", CourseGroupId = 1, TeacherId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1) };
            var assignment2 = new Assignment { Id = 2, Description = "Assignment 2", CourseGroupId = 1, TeacherId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(2) };

            _context.Students.Add(student);
            _context.Teachers.Add(teacher);
            _context.Groups.Add(group);
            _context.Courses.Add(course);
            _context.CourseGroups.Add(courseGroup);
            _context.AccountBooks.Add(accountBook);
            _context.Assignments.AddRange(assignment1, assignment2);

            _context.SaveChanges();

            _repository = new AssignmentRepository(_context);
        }

        [Test]
        public void GetAllAssignmentsOfStudent_Returns_Assignments_For_Student()
        {
            var student = _context.Students.First();

            var result = _repository.GetAllAssignmentsOfStudent(student);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => a.Description == "Assignment 1"));
            Assert.IsTrue(result.Any(a => a.Description == "Assignment 2"));
        }
    }
}
