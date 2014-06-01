using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniversityManagementSystemWebApplication.Models;

namespace UniversityManagementSystemWebApplication.Controllers
{
    public class TeacherController : Controller
    {
        private UniversityDBContext db = new UniversityDBContext();

        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.Designation).Include(t => t.Department);
            return View(teachers.ToList());
        }

        //
        // GET: /Teacher/Details/5

        public ActionResult Details(int id = 0)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        //
        // GET: /Teacher/Create

        public ActionResult Create()
        {
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            return View();
        }

        //
        // POST: /Teacher/Create

        [HttpPost]
        public ActionResult Create(Teacher teacher)
        {
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "Name", teacher.DesignationId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", teacher.DepartmentId);
            if (ModelState.IsValid)
            {
                teacher.RemainingCredit = teacher.CreditToBeTaken;
                db.Teachers.Add(teacher);
                db.SaveChanges();
                ViewBag.Message = teacher.Name + " has been saved in database.";
                return View();
            }
            return View();
        }

        //
        // GET: /Teacher/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "Name", teacher.DesignationId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", teacher.DepartmentId);
            return View(teacher);
        }

        //
        // POST: /Teacher/Edit/5

        [HttpPost]
        public ActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "Name", teacher.DesignationId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", teacher.DepartmentId);
            return View(teacher);
        }

        //
        // GET: /Teacher/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        //
        // POST: /Teacher/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult CheckTeacherEmail(string email)
        {
            var result = db.Teachers.Count(t => t.Email == email) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}