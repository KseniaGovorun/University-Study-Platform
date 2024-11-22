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
    public class CourseGroupRepository : Repository<CourseGroup>, ICourseGroupRepository
    {
        public CourseGroupRepository(ApplicationDbContext _db) : base(_db) { }

        public CourseGroup GetCourseGroupByAssignment(Assignment assignment)
        {
            var courseGroup = from c in db.CourseGroups
                              where assignment.CourseGroupId == c.Id
                              select c;

            return courseGroup.FirstOrDefault();
        }

    }
}
