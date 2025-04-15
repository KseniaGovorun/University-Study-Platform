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
    public class CourseRepositoryTests
    {
        private ApplicationDbContext _context;
        private CourseRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new CourseRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // --- Test for getting courses by student ---
        [Test]
        public void GetCoursesByStudent_ReturnsCourses_WhenStudentHasCourses()
        {
            var student = new Student { Id = 1 };
            var course = new Course { Id = 1 };
            var accountBook = new AccountBook { StudentId = 1, GroupId = 1 };
            var courseGroup = new CourseGroup { GroupId = 1, CourseId = 1, PhotoUrl = "", Title = "" };

            _context.Students.Add(student);
            _context.Courses.Add(course);
            _context.AccountBooks.Add(accountBook);
            _context.CourseGroups.Add(courseGroup);
            _context.SaveChanges();

            var result = _repository.GetCoursesByStudent(student);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(course.Id, result[0].Id);
        }

        // --- Test for getting courses by teacher ---
        [Test]
        public void GetCoursesByTeacher_ReturnsCourses_WhenTeacherHasCourses()
        {
            var teacher = new Teacher { Id = 1 };
            var course = new Course { Id = 1, TeacherId = teacher.Id };

            _context.Teachers.Add(teacher);
            _context.Courses.Add(course);
            _context.SaveChanges();

            var result = _repository.GetCoursesByTeacher(teacher.Id);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(course.Id, result[0].Id);
        }

        // --- Test for getting courses by term ---
        [Test]
        public void GetCoursesByTerm_ReturnsCourses_WhenCoursesExistForTerm()
        {
            var term = new Term { Id = 1, Name = "First Term" };
            var course = new Course { Id = 1, TermId = term.Id };

            _context.Terms.Add(term);
            _context.Courses.Add(course);
            _context.SaveChanges();

            var result = _repository.GetCoursesByTerm(term.Id);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(course.Id, result[0].Id);
        }

        // --- Test for getting courses by subject ---
        [Test]
        public void GetCoursesBySubject_ReturnsCourses_WhenCoursesExistForSubject()
        {
            var subject = new Subject { Id = 1, Name = "Maths" };
            var course = new Course { Id = 1, SubjectId = subject.Id };

            _context.Subjects.Add(subject);
            _context.Courses.Add(course);
            _context.SaveChanges();

            var result = _repository.GetCoursesBySubject(subject.Id);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(course.Id, result[0].Id);
        }

        // --- Test for getting messages for a course ---
        [Test]
        public void GetMessagesForCourse_ReturnsMessages_WhenCourseHasMessages()
        {
            var course = new Course { Id = 1 };
            var message = new Message { Id = 1, CourseGroupId = 1, Body = "Test message" };
            var courseGroup = new CourseGroup { CourseId = course.Id, GroupId = 1, PhotoUrl = "", Title = "" };

            _context.Courses.Add(course);
            _context.Messages.Add(message);
            _context.CourseGroups.Add(courseGroup);
            _context.SaveChanges();

            var result = _repository.GetMessagesForCourse(course.Id);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(message.Id, result[0].Id);
        }

        // --- Test for getting course with most students ---
        [Test]
        public void GetCourseWithMostStudents_ReturnsCorrectCourse()
        {
            var course1 = new Course { Id = 1 };
            var course2 = new Course { Id = 2 };
            var courseGroup1 = new CourseGroup {CourseId = 1, GroupId = 1, PhotoUrl = "", Title = "" };
            var courseGroup2 = new CourseGroup {CourseId = 1, GroupId = 2, PhotoUrl = "", Title = "" };
            var courseGroup3 = new CourseGroup {CourseId = 2, GroupId = 3, PhotoUrl = "", Title = "" };

            _context.Courses.AddRange(course1, course2);
            _context.CourseGroups.AddRange(courseGroup1, courseGroup2, courseGroup3);
            _context.SaveChanges();

            var result = _repository.GetCourseWithMostStudents();

            Assert.NotNull(result);
            Assert.AreEqual(course1.Id, result.Id);
        }
    }
}
