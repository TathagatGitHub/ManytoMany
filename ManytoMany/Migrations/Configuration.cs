namespace ManytoMany.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ManytoMany.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ManytoMany.Models.SchoolDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ManytoMany.Models.SchoolDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var students = new List <Student>
            {
                new Student { StudentId = 1,   StudentName="Tathagat Dwivedi" },
                new Student { StudentId = 2,   StudentName="Vintee Tripathi" } 
            };
            students.ForEach(s => context.Students.AddOrUpdate(p => p.StudentName, s));
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course { CourseId= 1,   CourseName="Science" ,Students = new List<Student>()},
                new Course { CourseId= 2,   CourseName="English" ,Students = new List<Student>()},
                
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseId, s));
            context.SaveChanges();
        }
    }
}
