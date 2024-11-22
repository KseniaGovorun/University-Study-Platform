using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository.IRepository;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.UnitOfWork.Repository
{
    public class StudentPerfomanceRepository : Repository<StudentPerfomance>, IStudentPerfomanceRepository
    {
        public StudentPerfomanceRepository(ApplicationDbContext _db) : base(_db) { }

        public List<StudentPerfomance> GetStudentPerfomanceByStudent(Student student)
        {
            var studentPerfomances = from c in db.StudentPerfomances
                          where c.AccountBookId == (from b in db.AccountBooks
                                              where b.StudentId == student.Id
                                              select b.Id).FirstOrDefault()
                          select c;

            return studentPerfomances.ToList();
        }
    }
}
