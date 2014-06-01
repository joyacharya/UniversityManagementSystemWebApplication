using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityManagementSystemWebApplication.Models;

namespace UniversityManagementSystemWebApplication.Controllers
{
    public class ViewCourseStateController : Controller
    {
        private UniversityDBContext db = new UniversityDBContext();
        //
        // GET: /ViewCourseState/

        public ActionResult Index(int? departmentId)
        {
            try
            {
                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
                var model = db.Courses.Include(d => d.Department).Where(c => c.Department.DepartmentId == departmentId).Include(c => c.Semester).Include(c => c.Teacher);
                return View(model.ToList());
            }
            catch
            {
                return View();
            }
        }


        public PartialViewResult FilteredDepartment(int? departmentId)
        {
            var model = db.Courses.Include(d => d.Department).Where(c => c.Department.DepartmentId == departmentId).Include(c => c.Semester).Include(c => c.Teacher);
            //var model = db.CourseAssignToTeachers.Include("Department").Include("Course").Where(c => c. == departmentId);
            return PartialView("_CourseInformation", model.ToList());
        }

    }
}
