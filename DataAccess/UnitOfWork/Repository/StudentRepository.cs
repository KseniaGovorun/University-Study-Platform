using Microsoft.EntityFrameworkCore;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext _db) : base(_db) { }

        public Student GetStudentByPersonId(int id)
        {
            return db.Students.Include(s => s.Person).FirstOrDefault(s => s.PersonId == id);
        }

        // ✅ Метод: Отримати всі оцінки (StudentPerformance) для студента
        public IEnumerable<StudentPerfomance> GetStudentPerformanceByStudentId(int studentId)
        {
            return db.StudentPerfomances
                     .Include(sp => sp.Subject)
                     .Include(sp => sp.AccountBook)
                     .Where(sp => sp.AccountBook.StudentId == studentId)
                     .ToList();
        }

        // ✅ Метод: Обрахувати середній бал студента
        public double CalculateStudentTotalGPA(int studentId)
        {
            var performances = GetStudentPerformanceByStudentId(studentId);
            if (!performances.Any()) return 0;

            return performances.Average(p => p.TotalPoint);
        }
    }
}
