using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public int CourseGroupId { get; set; }
        public CourseGroup CourseGroup { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
