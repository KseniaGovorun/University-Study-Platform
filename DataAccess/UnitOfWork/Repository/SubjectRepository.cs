using Microsoft.EntityFrameworkCore;
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
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext _db) : base(_db) { }

        // Отримати предмет за назвою
        public Subject GetByName(string name)
        {
            return db.Subjects.FirstOrDefault(s => s.Name == name);
        }

        // Отримати список усіх курсів для предмету
        public IEnumerable<Course> GetCoursesBySubjectId(int subjectId)
        {
            return db.Courses
                     .Include(c => c.Teacher)
                     .Include(c => c.CreditForm)
                     .Include(c => c.Term)
                     .Where(c => c.SubjectId == subjectId)
                     .ToList();
        }

        // Отримати всі оцінки студентів для певного предмету
        public IEnumerable<StudentPerfomance> GetPerfomanceBySubjectId(int subjectId)
        {
            return db.StudentPerfomances
                     .Include(p => p.AccountBook)
                     .ThenInclude(ab => ab.Student)
                     .Where(p => p.SubjectId == subjectId)
                     .ToList();
        }

        // Порахувати середній бал по предмету
        public double CalculateAverageGradeForSubject(int subjectId)
        {
            var perfomances = GetPerfomanceBySubjectId(subjectId);
            if (!perfomances.Any()) return 0;

            return perfomances.Average(p => p.TotalPoint);
        }

        // Отримати всі розклади (пари) по предмету
        public IEnumerable<Shedule> GetSheduleBySubjectId(int subjectId)
        {
            return db.Shedule
                     .Include(s => s.Group)
                     .Include(s => s.Teacher)
                     .Where(s => s.SubjectId == subjectId)
                     .ToList();
        }
    }

}
