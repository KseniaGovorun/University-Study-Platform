using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class StudentPerfomance
    {
        public int StudentPerfomanceId { get; set; }

        public int AccountBookId { get; set; }
        public AccountBook AccountBook { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public float CurrentPoint { get; set; }
        public float ExamPoint { get; set; }
        public float TotalPoint { get; set; }
        public int SemesterNumber { get; set; }
    }
}
