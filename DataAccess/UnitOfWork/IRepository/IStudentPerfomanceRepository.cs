using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityStudyPlatform.Models;

namespace UniversityStudyPlatform.DataAccess.Repository.IRepository
{
    public interface IStudentPerfomanceRepository : IRepository<StudentPerfomance>
    {
        public List<StudentPerfomance> GetStudentPerfomanceByStudent(Student student);
    }
}
