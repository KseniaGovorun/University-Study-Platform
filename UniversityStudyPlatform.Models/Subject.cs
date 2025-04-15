﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityStudyPlatform.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<StudentPerfomance> StudentPerfomances { get; set;}
        public ICollection<Course> Courses { get; set;}
        public ICollection<Shedule> Shedule { get; set;}
    }
}
