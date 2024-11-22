using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationDbContext _db) : base(_db) { }

        public List<Assignment> GetAllAssignmentsOfStudent(Student student)
        {

            var group1 = from b in db.AccountBooks
                         where b.StudentId == student.Id
                         select b;

            var courseGroups = from c in db.CourseGroups
                               where c.GroupId == group1.FirstOrDefault().Id
                               select c;

            List<Assignment> resultAssignments = new List<Assignment>();
            for (int i = 0; i < courseGroups.Count(); i++)
            {
                var assignments = from a in db.Assignments
                                  where a.CourseGroupId == courseGroups.ToList().ElementAt(i).Id
                                  select a;

                for (int j = 0; j < assignments.Count(); j++)
                {
                    resultAssignments.Add(assignments.ToList().ElementAt(j));
                }
            }

            return resultAssignments;
        }

        public CourseGroup GetCourseGroupOfAssignment(Assignment assignment)
        {
            var courseGroup = from c in db.CourseGroups
                              where assignment.CourseGroupId == c.Id
                              select c;

            return courseGroup.FirstOrDefault();
        }

    }
}
