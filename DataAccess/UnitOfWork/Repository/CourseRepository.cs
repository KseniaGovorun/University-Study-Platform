using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext _db) : base(_db) { }

        public List<Course> GetCoursesByStudent(Student student)
        {
            var courses = from c in db.CourseGroups
                          where c.GroupId == (from b in db.AccountBooks
                                              where b.StudentId == student.Id
                                              select b.GroupId).FirstOrDefault()
                          select c.Course;

            return courses.ToList();
        }
    }
}
