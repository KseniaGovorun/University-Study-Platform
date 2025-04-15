using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext _db) : base(_db) { }

        // Get all courses by a specific student
        public List<Course> GetCoursesByStudent(Student student)
        {
            var courses = from c in db.CourseGroups
                          where c.GroupId == (from b in db.AccountBooks
                                              where b.StudentId == student.Id
                                              select b.GroupId).FirstOrDefault()
                          select c.Course;

            return courses.ToList();
        }

        // Get all courses by teacher ID
        public List<Course> GetCoursesByTeacher(int teacherId)
        {
            var courses = from c in db.Courses
                          where c.TeacherId == teacherId
                          select c;

            return courses.ToList();
        }

        // Get all courses by term ID
        public List<Course> GetCoursesByTerm(int termId)
        {
            var courses = from c in db.Courses
                          where c.TermId == termId
                          select c;

            return courses.ToList();
        }

        // Get all courses by subject
        public List<Course> GetCoursesBySubject(int subjectId)
        {
            var courses = from c in db.Courses
                          where c.SubjectId == subjectId
                          select c;

            return courses.ToList();
        }

        // Get all messages for a specific course
        public List<Message> GetMessagesForCourse(int courseId)
        {
            var messages = from m in db.Messages
                           where m.CourseGroup.Course.Id == courseId
                           select m;

            return messages.ToList();
        }

        // Get the course with the most students enrolled
        public Course GetCourseWithMostStudents()
        {
            var course = db.CourseGroups
                           .GroupBy(cg => cg.CourseId)
                           .OrderByDescending(g => g.Count())
                           .Select(g => g.Key)
                           .FirstOrDefault();

            return db.Courses.FirstOrDefault(c => c.Id == course);
        }

        // Get the average number of students enrolled in a course
        public double GetAverageNumberOfStudents()
        {
            return db.CourseGroups
                     .GroupBy(cg => cg.CourseId)
                     .Average(g => g.Count());
        }
    }
}
