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
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext _db) : base(_db) { }

        public string GetTeacherNameById(int id)
        {
            var teacher = (from p in db.Persons
                           where p.Id == (from t in db.Teachers
                                          where t.Id == id
                                          select t).FirstOrDefault().PersonId
                           select p).FirstOrDefault();

            string fullName = teacher.Name + " " + teacher.Surname;

            return fullName;
        }

    }
}
