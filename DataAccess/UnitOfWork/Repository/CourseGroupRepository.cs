using Microsoft.EntityFrameworkCore;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class CourseGroupRepository : Repository<CourseGroup>, ICourseGroupRepository
    {
        public CourseGroupRepository(ApplicationDbContext _db) : base(_db) { }

        public CourseGroup GetCourseGroupByAssignment(Assignment assignment)
        {
            return db.CourseGroups
                     .FirstOrDefault(c => c.Id == assignment.CourseGroupId);
        }

        public List<CourseGroup> GetCourseGroupsByCourse(Course course)
        {
            return db.CourseGroups
                     .Where(cg => cg.CourseId == course.Id)
                     .ToList();
        }

        public List<CourseGroup> GetCourseGroupsByGroup(Group group)
        {
            return db.CourseGroups
                     .Where(cg => cg.GroupId == group.Id)
                     .ToList();
        }

        public CourseGroup GetCourseGroupByCourseAndGroup(int courseId, int groupId)
        {
            return db.CourseGroups
                     .FirstOrDefault(cg => cg.CourseId == courseId && cg.GroupId == groupId);
        }

        public bool CourseGroupExists(int courseId, int groupId)
        {
            return db.CourseGroups
                     .Any(cg => cg.CourseId == courseId && cg.GroupId == groupId);
        }

        public List<CourseGroup> GetAllWithCourseAndGroup()
        {
            return db.CourseGroups
                     .Include(cg => cg.Course)
                     .Include(cg => cg.Group)
                     .ToList();
        }
    }

}
