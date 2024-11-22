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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext _db) : base(_db) { }

        public Student GetStudentByPersonId(int id)
        {
            var student = from s in db.Students
                          where id == s.PersonId
                          select s;

            return student.FirstOrDefault();
        }
    }
}
