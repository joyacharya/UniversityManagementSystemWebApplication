using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniversityManagementSystemWebApplication.Models;

namespace UniversityManagementSystemWebApplication.Controllers
{
    public class CourseController : Controller
    {
        private UniversityDBContext db = new UniversityDBContext();
        //
        // GET: /Course/
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department).Include(c => c.Semester);
            return View(courses.ToList());
        }
        //
        // GET: /Course/Details/5
        public ActionResult Details(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        //
        // GET: /Course/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
            return View();
        }
        //
        // POST: /Course/Create
        [HttpPost]
        public ActionResult Create(Course course)
        {
            PopulateDropdownList(course);
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                ViewBag.Message = "This course " + course.Name + " has been saved.";
                return View(course);
            }

            return View(course);
        }

        private void PopulateDropdownList(Course course)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name", course.SemesterId);
        }

        //
        // GET: /Course/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            PopulateDropdownList(course);
            return View(course);
        }
        //
        // POST: /Course/Edit/5
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropdownList(course);
            return View(course);
        }
        //
        // GET: /Course/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        //
        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult CheckCourseCode(string code)
        {
            var result = db.Courses.Count(c => c.Code == code) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCourseName(string name)
        {
            var result = db.Courses.Count(c => c.Name == name) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CourseAssign()
        {
            PopulateDropdownList();
            ViewBag.Message = "";
            return View();
        }

        private void PopulateDropdownList()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.TeacherId = new SelectList("", "TeacherId", "Name");
            ViewBag.CourseId = new SelectList("", "CourseId", "Code");
        }

        //
        // POST: /Course/Create
        [HttpPost]
        public ActionResult CourseAssign(Department aDepartment, Course aCourse, Teacher aTeacher)
        {
            PopulateDropdownList();
            if (aDepartment.DepartmentId == 0 || aCourse.CourseId == 0 || aTeacher.TeacherId == 0)
            {
                ViewBag.Message = "All fields are required";
                return View();
            }

            aDepartment = db.Departments.Find(aDepartment.DepartmentId);
            aTeacher = db.Teachers.Find(aTeacher.TeacherId);
            aCourse = db.Courses.Find(aCourse.CourseId);

            if (aCourse.Teacher != null)
            {
                PopulateDrodownList(aCourse,aTeacher);
                ViewBag.Message = "Course " + aCourse.Code + " already assigned. Please take another course for "+aTeacher.Name;
                return View();

            }
            aCourse.Teacher = aTeacher;
            aCourse.Department = aDepartment;

            if (!ModelState.IsValid)
            {
                db.Entry(aCourse).State = EntityState.Modified;
                db.SaveChanges();
                aTeacher.RemainingCredit -= aCourse.Credit;
                db.Entry(aTeacher).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Success = "Course " + aCourse.Code + " assigned to " + aTeacher.Name;
                return View();
            }
            return View();
        }


        private void PopulateDrodownList(Course aCourse, Teacher aTeacher)
        {
            var teachers = db.Teachers.Where(t => t.DepartmentId == aTeacher.DepartmentId).ToList();
            var courses = db.Courses.Where(s => s.DepartmentId == aCourse.DepartmentId).ToList();
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.TeacherId = new SelectList(teachers, "TeacherId", "Name", aTeacher.Name);
            ViewBag.CourseId = new SelectList(courses, "CourseId", "Code", aCourse.Code);
        }

        public ActionResult SelectDepartmentForTeacher(int? departmentId)
        {
            var teachers = db.Teachers.Where(t => t.DepartmentId == departmentId);
            ViewBag.TeacherId = new SelectList(teachers.ToArray(), "TeacherId", "Name");
            return PartialView("_Teacher", ViewData["TeacherId"]);
        }
        public ActionResult SelectDepartmentForCourse(int? departmentId)
        {
            var courses = db.Courses.Where(s => s.DepartmentId == departmentId);
            ViewBag.CourseId = new SelectList(courses.ToArray(), "CourseId", "Code");
            return PartialView("_Course", ViewData["CourseId"]);
        }
        public ActionResult SelectTeacher(int? teacherId)
        {
            Teacher teacher = db.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);
            /*Course aCourse = new Course();
            aCourse.Teacher = teacher;*/
            return PartialView("_TeacherDetails", teacher);
        }
        public ActionResult SelectCourse(int? courseId)
        {
            Course course = db.Courses.FirstOrDefault(c => c.CourseId == courseId);
            return PartialView("_CourseDetails", course);
        }

    }
}