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
    using NUnit.Framework;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    [TestFixture]
    public class CourseGroupRepositoryTests
    {
        private ApplicationDbContext _context;
        private CourseGroupRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var course = new Course { Id = 1 };
            var group = new Group { Id = 1, Name = "Group" };
            var courseGroup = new CourseGroup
            {
                Id = 1,
                Title = "Test Group",
                PhotoUrl = "photo.jpg",
                CourseId = course.Id,
                GroupId = group.Id,
                Course = course,
                Group = group
            };

            _context.Courses.Add(course);
            _context.Groups.Add(group);
            _context.CourseGroups.Add(courseGroup);
            _context.Assignments.Add(new Assignment { Id = 1, CourseGroupId = 1, Description = "Description" });

            _context.SaveChanges();

            _repository = new CourseGroupRepository(_context);
        }

        [Test]
        public void GetCourseGroupByAssignment_Returns_Correct_CourseGroup()
        {
            var assignment = _context.Assignments.First();

            var result = _repository.GetCourseGroupByAssignment(assignment);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void GetCourseGroupsByCourse_Returns_Correct_List()
        {
            var course = _context.Courses.First();

            var result = _repository.GetCourseGroupsByCourse(course);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetCourseGroupsByGroup_Returns_Correct_List()
        {
            var group = _context.Groups.First();

            var result = _repository.GetCourseGroupsByGroup(group);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetCourseGroupByCourseAndGroup_Returns_Correct_CourseGroup()
        {
            var result = _repository.GetCourseGroupByCourseAndGroup(1, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void CourseGroupExists_Returns_True_If_Exists()
        {
            var exists = _repository.CourseGroupExists(1, 1);

            Assert.IsTrue(exists);
        }

        [Test]
        public void GetAllWithCourseAndGroup_Returns_List_With_Navigation_Properties()
        {
            var result = _repository.GetAllWithCourseAndGroup();

            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result.First().Course);
            Assert.IsNotNull(result.First().Group);
        }
    }
}
