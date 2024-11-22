using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Student
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
