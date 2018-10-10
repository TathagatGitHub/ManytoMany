using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManytoMany.Models
{
    public class StudentCourse
    {
        public int StudentCourseID { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}