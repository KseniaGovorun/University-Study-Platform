using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class StudentIndividualTask
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool isDone { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public int AccountBookId { get; set; }
        public AccountBook AccountBook { get; set; }
    }
}
