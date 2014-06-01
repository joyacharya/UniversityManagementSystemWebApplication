using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityManagementSystemWebApplication.Models;
using RazorPDF;
using iTextSharp;
using iTextSharp.text;

namespace UniversityManagementSystemWebApplication.Controllers
{
    public class EnrollmentController : Controller
    {
        private UniversityDBContext db = new UniversityDBContext();

        //
        // GET: /Enrollment/

        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Student).Include(e => e.Course);
            return View(enrollments.ToList());
        }

        //
        // GET: /Enrollment/Details/5

        public ActionResult Details(int id = 0)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        //
        // GET: /Enrollment/Create

        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            ViewBag.CourseId = new SelectList("", "CourseId", "Code");
            return View();
        }

        //
        // POST: /Enrollment/Create

        [HttpPost]
        public ActionResult Create(Student aStudent,Course aCourse,DateTime date)
        {
            Create();
            if (aStudent.StudentId == 0 || aCourse.CourseId == 0)
            {
                ViewBag.ErrorMessage = "All fields are required.";
                return View();
            }
            Enrollment enrollment = new Enrollment();
            enrollment.StudentId = aStudent.StudentId;
            enrollment.CourseId = aCourse.CourseId;
            enrollment.Date = date;
            Student student = (db.Students.Where(s => s.StudentId == aStudent.StudentId)).Single();
            Course course = db.Courses.Single(c => c.CourseId == aCourse.CourseId);
            bool check = db.Enrollments.Count(c => c.StudentId == aStudent.StudentId && c.CourseId == aCourse.CourseId) == 0;
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                ViewBag.Message = course.Code + " has assigned for " + student.RegNo;
                return View(enrollment);
            }
            
            enrollment.Student = student;
            enrollment.Course = course;
            ViewBag.ErrorMessage = student.RegNo+" has already enrolled for "+course.Code;
            return View(enrollment);
        }

        //
        // GET: /Enrollment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo", enrollment.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", enrollment.CourseId);
            return View(enrollment);
        }

        //
        // POST: /Enrollment/Edit/5

        [HttpPost]
        public ActionResult Edit(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo", enrollment.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", enrollment.CourseId);
            return View(enrollment);
        }

        //
        // GET: /Enrollment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        //
        // POST: /Enrollment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult SelectCourseForStudent(int? studentId)
        {
            Student student = (db.Students.Where(s => s.StudentId == studentId)).Single();
            //List<Course> queryable = db.Courses.Where(c => c.DepartmentId == student.DepartmentId).ToList();
            var courses = db.Courses.Where(c => c.DepartmentId == student.DepartmentId);
            ViewBag.CourseId = new SelectList(courses.ToArray(), "CourseId", "Code");
            return PartialView("_Course", ViewData["CourseId"]);
        }

        public ActionResult SelectEnrolledCourseForStudent(int? studentId)
        {
            var courses = GetEnrolledCourses(studentId);
            ViewBag.CourseId = new SelectList(courses.ToArray(), "CourseId", "Code");
            return PartialView("_Course", ViewData["CourseId"]);
        }

        private List<Course> GetEnrolledCourses(int? studentId)
        {
            List<Enrollment> enrollments = db.Enrollments.Where(e => e.StudentId == studentId).ToList();
            List<Course> courseList = db.Courses.ToList();
            List<Course> courses = new List<Course>();
            foreach (Course course in courseList)
            {
                foreach (Enrollment enrollment in enrollments)
                {
                    if (enrollment.CourseId == course.CourseId)
                    {
                        courses.Add(enrollment.Course);
                    }
                }
            }
            return courses;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult SelectStudent(int? studentId)
        {
            Student student = (db.Students.Where(s => s.StudentId == studentId)).Single();
            Enrollment enrollment = new Enrollment();
            enrollment.Student = student;
            return PartialView("~/Views/Shared/_StudentDetails.cshtml", enrollment);
        }

        public ActionResult StudentResultEntry()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            ViewBag.CourseId = new SelectList("", "CourseId", "Code");
            ViewBag.GradeLetterId = new SelectList(db.GradeLetters, "GradeLetterId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult StudentResultEntry(Enrollment enrollment)
        {
            LoadDropdownList(enrollment);
            
            if (enrollment.StudentId == 0 || enrollment.CourseId == 0 || enrollment.GradeLetterId == null)
            {
                ViewBag.Message = "All fields are required.";
                return View();
            }
            GradeLetter gradeLetter = (db.GradeLetters.Where(g => g.GradeLetterId == enrollment.GradeLetterId)).Single();
            Course course = (db.Courses.Where(c => c.CourseId == enrollment.CourseId)).Single();
            Student student = (db.Students.Where(s => s.StudentId == enrollment.StudentId)).Single();
            Enrollment enrolled = (db.Enrollments.Where(e => (e.StudentId == student.StudentId && e.CourseId == course.CourseId))).Single();

            bool check = db.Enrollments.Count(e => (e.StudentId == student.StudentId && e.CourseId == course.CourseId && e.GradeLetter != null)) == 1;
            enrollment = enrolled;
            enrollment.GradeLetter = gradeLetter;
            enrollment.GradeLetterId = gradeLetter.GradeLetterId;
            enrollment.Student = enrolled.Student;
            enrollment.StudentId = enrolled.StudentId;
            enrollment.Course = course;
            if (!check)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                ViewBag.Saved = student.Name+" got "+gradeLetter.Name+" in "+course.Name;
                return View(enrollment);
            }
            
            ViewBag.Message = course.Name + " course grade already assigned for "+student.Name;
            return View(enrollment);
        }

        private void LoadDropdownList(Enrollment enrollment)
        {
            List<Course> enrolledCourses = GetEnrolledCourses(enrollment.StudentId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            ViewBag.CourseId = new SelectList(enrolledCourses, "CourseId", "Code");
            ViewBag.GradeLetterId = new SelectList(db.GradeLetters, "GradeLetterId", "Name");
        }

        public ActionResult ViewResult(int? studentId)
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            var enrollments = db.Enrollments.Where(e => e.StudentId == studentId);
            return View(enrollments.ToList());
        }

        [HttpPost]
        public ActionResult ViewResult(int studentId)
        {
            var enrollments = db.Enrollments.Where(e => e.StudentId == studentId);
            var pdf = new PdfResult(enrollments.ToArray(), "ViewPDFResult");
            return pdf;
        }

        public PartialViewResult SelectEnrolledStudentInformation(int? studentId)
        {
            var enrollments = db.Enrollments.Where(e => e.StudentId == studentId);
            return PartialView("_CourseInformation", enrollments.ToList());
        }

        public ActionResult SelectStudentDetails(int? studentId)
        {
            var enrollments = db.Enrollments.Include(e => e.Student).Where(e => e.StudentId == studentId).Include(e => e.Course).Include(e => e.GradeLetter);
            return PartialView("_StudentDetails", enrollments.ToList());
        }
    }
}