using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Course
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int CreditFormId { get; set; }
        public CreditForm CreditForm { get; set; }

        public int TermId { get; set; }
        public Term Term { get; set; }

        ICollection<CourseGroup> CourseGroups { get; set; }
        ICollection<Assignment> Assignments { get; set; }
        ICollection<Message> Messages { get; set; }
    }
}
