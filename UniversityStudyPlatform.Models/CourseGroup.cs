using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class CourseGroup
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string PhotoUrl { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
