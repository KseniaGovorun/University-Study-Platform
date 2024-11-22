using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int StudentAmount { get; set; }

        public ICollection<AccountBook> AccountBooks { get; set; }
        public ICollection<CourseGroup> CourseGroups { get; set; }
        public ICollection<Shedule> Shedule { get; set; }
    }
}
