using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniversityManagementSystemWebApplication.Models;

namespace UniversityManagementSystemWebApplication.Controllers
{
    public class StudentController : Controller
    {
        private UniversityDBContext db = new UniversityDBContext();

        //
        // GET: /Student/

        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        //
        // GET: /Student/Details/5

        public ActionResult Details(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            return View();
        }

        //
        // POST: /Student/Create
        [HttpPost]
        public ActionResult Create(Student student)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", student.DepartmentId);
            if (ModelState.IsValid)
            {
                int count = db.Students.Count(s => s.DepartmentId == student.DepartmentId & s.Date.Year == student.Date.Year);
                //student.RegNo = "OOP0140" + (count+1);
                Department department = db.Departments.Find(student.DepartmentId);
                student.RegNo = department.Code + student.Date.Year + (count + 1).ToString("D3");
                db.Students.Add(student);
                db.SaveChanges();
                ViewBag.Message = student.Name + "'s registration number is " + student.RegNo;
                return View();
            }

            return View(student);
        }

        //
        // GET: /Student/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", student.DepartmentId);
            return View(student);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", student.DepartmentId);
            return View(student);
        }

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult CheckMail(string email)
        {
            var result = db.Students.Count(s => s.Email == email) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}