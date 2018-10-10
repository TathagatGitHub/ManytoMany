using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManytoMany.Models
{
    public class CoursesViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<CheckBoxViewModel> Students { get; set; }
    }
}