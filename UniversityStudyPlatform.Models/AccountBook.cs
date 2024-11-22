using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class AccountBook
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<StudentPerfomance> StudentPerfomances { get; set; }
        ICollection<StudentIndividualTask> StudentIndividualTask { get; set; }
    }
}
