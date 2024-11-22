using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Shedule
    {
        public int Id { get; set; }
        [Required]
        public string Faculty { get; set; }
        [Required]
        public Day Day { get; set; }
        [Required]
        public int NumberPara { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
