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
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext _db) : base(_db) { }

        public string GetTeacherNameById(int id)
        {
            var teacher = db.Teachers
                            .Include(t => t.Person)
                            .FirstOrDefault(t => t.Id == id);

            if (teacher == null || teacher.Person == null)
                return null;

            string fullName = teacher.Person.Name + " " + teacher.Person.Surname;

            return fullName;
        }

    }
}
