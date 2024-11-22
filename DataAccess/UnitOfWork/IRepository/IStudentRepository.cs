using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        public Student GetStudentByPersonId(int id);
    }
}
