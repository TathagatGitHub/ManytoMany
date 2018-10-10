using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManytoMany.Models;

namespace ManytoMany.Controllers
{
    public class CoursesController : Controller
    {
        private SchoolDBContext db = new SchoolDBContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var Results = from s in db.Students
                          select new
                          {
                              s.StudentId,
                              s.StudentName,
                              Checked = ((from sc in db.StudentCourse
                                          where (sc.CourseId == id) & (sc.StudentId == s.StudentId)
                                          select sc).Count() > 0)
                          };
            var MyViewModel = new CoursesViewModel();
            MyViewModel.CourseId = id.Value;
            MyViewModel.CourseName = course.CourseName;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.StudentId, Name = item.StudentName, Checked = item.Checked });
            }
            MyViewModel.Students = MyCheckBoxList;
            return View(MyViewModel);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var Results = from s in db.Students
                          select new
                          {
                              s.StudentId,
                              s.StudentName,
                              Checked =((from sc in db.StudentCourse where (sc.CourseId==id) & (sc.StudentId==s.StudentId)
                                         select sc).Count()>0)
                          };
            var MyViewModel = new CoursesViewModel();
            MyViewModel.CourseId = id.Value;
            MyViewModel.CourseName = course.CourseName;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.StudentId, Name = item.StudentName, Checked = item.Checked });
            }
            MyViewModel.Students = MyCheckBoxList;
            return View(MyViewModel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "CourseId,CourseName")] Course course)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(course).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(course);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoursesViewModel course)
        {

            if (ModelState.IsValid)
            {
                var MyCourse = db.Courses.Find(course.CourseId);
                MyCourse.CourseName = course.CourseName;
                
                //foreach (var item in db.StudentCourse)
                //{
                //    if (item.CourseId==course.CourseId)
                //    {
                //        db.Entry(course).State = EntityState.Deleted;
                //    }
                //}
                foreach (var item in course.Students )
                {
                    if (item.Checked)
                    {
                        var studentCourse = from sc in db.StudentCourse
                                            where (sc.CourseId == course.CourseId) & (sc.StudentId == item.Id)
                                            select sc;
                        
                            if (studentCourse.Count() ==0)
                            {

                                db.StudentCourse.Add(new StudentCourse() { CourseId = course.CourseId, StudentId = item.Id });

                            }
                        
                       

                    }
                    else
                     {
                        var studentCourse = from sc in db.StudentCourse
                                                      where(sc.CourseId == course.CourseId) & (sc.StudentId == item.Id)
                                                      select sc;
                        foreach (var s in studentCourse)
                        {
                            StudentCourse st = db.StudentCourse.Find(s.StudentCourseID);
                            db.StudentCourse.Remove(st);
                            //db.StudentCourse.Remove(new StudentCourse() { CourseId = course.CourseId, StudentId = item.Id });
                        }
                    }
                }

                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
