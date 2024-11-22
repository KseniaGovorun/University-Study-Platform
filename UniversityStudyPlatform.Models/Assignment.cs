using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int CourseGroupId { get; set; }
        public CourseGroup CourseGroup { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        ICollection<StudentIndividualTask> StudentIndividualTask { get; set; }
    }
}
