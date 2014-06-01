using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityManagementSystemWebApplication.Models;

namespace UniversityManagementSystemWebApplication.Controllers
{
    public class RoomAllocationController : Controller
    {
        private UniversityDBContext db = new UniversityDBContext();

        //
        // GET: /RoomAllocation/

        public ActionResult Index()
        {
            var classroomallocations = db.ClassRoomAllocations.Include(c => c.Department).Include(c => c.Course).Include(c => c.ClassRoom).Include(c => c.Day);
            return View(classroomallocations.ToList());
        }

        //
        // GET: /RoomAllocation/Details/5

        public ActionResult Details(int id = 0)
        {
            ClassRoomAllocation classroomallocation = db.ClassRoomAllocations.Find(id);
            if (classroomallocation == null)
            {
                return HttpNotFound();
            }
            return View(classroomallocation);
        }

        //
        // GET: /RoomAllocation/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.CourseId = new SelectList("", "CourseId", "Code");
            ViewBag.ClassRoomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo");
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name");
            return View();
        }

        //
        // POST: /RoomAllocation/Create

        [HttpPost]
        public ActionResult Create(ClassRoomAllocation classRoomAllocation)
        {
            Create();
            if (classRoomAllocation.DepartmentId == 0 || classRoomAllocation.CourseId == 0 || classRoomAllocation.ClassRoomId == 0 || classRoomAllocation.DayId == 0)
            {
                return View();
            }
            string timeFrom = classRoomAllocation.TimeFrom;
            string timeTo = classRoomAllocation.TimeTo;

            Course aCourse = (db.Courses.Where(course => course.CourseId == classRoomAllocation.CourseId)).Single();
            Department aDepartment = (db.Departments.Where(d => d.DepartmentId == aCourse.DepartmentId)).Single();
            Day day = db.Days.Single(d => d.DayId == classRoomAllocation.DayId);
            ClassRoom classRoom = db.ClassRooms.Single(c => c.ClassRoomId == classRoomAllocation.ClassRoomId);
            List<ClassRoomAllocation> allocationRooms = (db.ClassRoomAllocations.Where(c =>
                (c.DayId == classRoomAllocation.DayId &&
                 c.ClassRoomId == classRoomAllocation.ClassRoomId))).ToList();

            List<ClassRoomAllocation> allocationCourses =
                db.ClassRoomAllocations.Where(
                    c => (c.DayId == classRoomAllocation.DayId && c.CourseId == classRoomAllocation.CourseId)).ToList();
            string timeFrom2 = "";
            string timeTo2 = "";
            int id=0;
            bool confirm = timeFrom == timeTo;

            foreach (ClassRoomAllocation room in allocationRooms)
            {
                bool check = CheckOverlapping(timeFrom, timeTo, room.TimeFrom, room.TimeTo);
                if (check)
                {
                    confirm = true;
                    timeFrom2 = room.TimeFrom;
                    timeTo2 = room.TimeTo;
                    id = room.CourseId;
                }
            }

            foreach (ClassRoomAllocation room in allocationCourses)
            {
                bool check = CheckOverlapping(timeFrom, timeTo, room.TimeFrom, room.TimeTo);
                if (check)
                {
                    confirm = true;
                }
            }

            if (!confirm)
            {
                LoadDropDownList(classRoomAllocation);
                classRoomAllocation.Department = aDepartment;
                if (ModelState.IsValid)
                {
                    db.ClassRoomAllocations.Add(classRoomAllocation);
                    db.SaveChanges();
                    if (aCourse.Teacher == null)
                    {
                        ViewBag.Success = aCourse.Code + " has been allocated in " + classRoom.RoomNo +
                                          " class room. And the schedual is " + day.Name + " at " +
                                          classRoomAllocation.TimeFrom + "-" + classRoomAllocation.TimeTo +
                                          " and teacher is not assigned yet.";
                    }
                    else
                    {
                        ViewBag.Success = aCourse.Code + " has been allocated in " + classRoom.RoomNo +
                                          " class room. And the schedual is " + day.Name + " at " +
                                          classRoomAllocation.TimeFrom + "-" + classRoomAllocation.TimeTo +
                                          " and the teacher of this course is" + aCourse.Teacher.Name;
                    }
                    return View();
                }
            }
            Course course2 = db.Courses.Single(c => c.CourseId == id);
            LoadDropDownList(classRoomAllocation);
            if (course2.Teacher == null)
            {
                ViewBag.ErrorMessage = "This Room: " + classRoom.RoomNo + " has already allocated at " + timeFrom2 + "-" + timeTo2 + " for " + course2.Code +
                                   " and teacher is not assigned yet.";
            }
            else
            {
                ViewBag.ErrorMessage = "This Room: " + classRoom.RoomNo + " has already allocated at " + timeFrom2 + "-" + timeTo2 + " for " + course2.Code +
                                   " and the teacher of this course is "+course2.Teacher.Name;
            }
            
            return View(classRoomAllocation);
        }

        private void LoadDropDownList(ClassRoomAllocation classRoomAllocation)
        {
            var courses = db.Courses.Where(s => s.DepartmentId == classRoomAllocation.DepartmentId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", classRoomAllocation.DepartmentId);
            ViewBag.CourseId = new SelectList(courses.ToArray(), "CourseId", "Code", classRoomAllocation.CourseId);
            ViewBag.ClassRoomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", classRoomAllocation.ClassRoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", classRoomAllocation.DayId);
        }

        private bool CheckOverlapping(string givenTimeFrom, string givenTimeTo, string currentTimeFrom, string currentTimeTo)
        {
            int[] timeFrom = new int[10000];
            int[] timeTo = new int[10000];

            for (int i = 0; i < 9995; i++)
            {
                timeFrom[i] = 0;
                timeTo[i] = 0;
            }

            int givenTF = TimeConversion(givenTimeFrom);
            int givenTT = TimeConversion(givenTimeTo);
            int currentTF = TimeConversion(currentTimeFrom);
            int currentTT = TimeConversion(currentTimeTo);

            if (givenTF > givenTT)
            {
                for (int i = 0; i <= givenTT; i++)
                {
                    timeFrom[i] += 1;
                }
                for (int i = givenTT; i <= 1439; i++)
                {
                    timeFrom[i] += 1;
                }
            }
            for (int i = givenTF; i <= givenTT; i++)
            {
                timeFrom[i] += 1;
            }
            if (currentTF > currentTT)
            {
                for (int i = 0; i <= currentTT; i++)
                {
                    timeFrom[i] += 1;
                }
                for (int i = currentTT; i <= 1439; i++)
                {
                    timeFrom[i] += 1;
                }
            }
            for (int i = currentTF; i <= currentTT; i++)
            {
                timeFrom[i] += 1;
            }

            Boolean confirmed = false;

            for (int i = 0; i < 1440; i++)
            {
                if (timeFrom[i] > 1)
                {
                    confirmed = true;
                }
            }
            return confirmed;
        }

        private int TimeConversion(string givenTimeFrom)
        {
            int hour = 0;
            int min = 0;
            int count = 0;

            for (int i = 0; i < givenTimeFrom.Length; i++)
            {
                if (givenTimeFrom[i] >='0' && givenTimeFrom[i] <='9')
                {
                    if (count == 0)
                    {
                        hour *= 10;
                        hour += (givenTimeFrom[i] - '0');
                    }
                    if (count == 1)
                    {
                        min *= 10;
                        min += (givenTimeFrom[i] - '0');
                    }
                }
                else
                {
                    count++;
                }
            }
            hour *= 60;
            min += hour;
            return min;
        }


        public ActionResult SelectDepartmentForCourse(int? departmentId)
        {
            var courses = db.Courses.Where(s => s.DepartmentId == departmentId);
            ViewBag.CourseId = new SelectList(courses.ToArray(), "CourseId", "Code");
            return PartialView("_Course", ViewData["CourseId"]);
        }
        

        //
        // GET: /RoomAllocation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ClassRoomAllocation classroomallocation = db.ClassRoomAllocations.Find(id);
            if (classroomallocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", classroomallocation.DepartmentId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", classroomallocation.CourseId);
            ViewBag.ClassRoomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", classroomallocation.ClassRoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", classroomallocation.DayId);
            return View(classroomallocation);
        }

        //
        // POST: /RoomAllocation/Edit/5

        [HttpPost]
        public ActionResult Edit(ClassRoomAllocation classroomallocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classroomallocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", classroomallocation.DepartmentId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", classroomallocation.CourseId);
            ViewBag.ClassRoomId = new SelectList(db.ClassRooms, "ClassRoomId", "RoomNo", classroomallocation.ClassRoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", classroomallocation.DayId);
            return View(classroomallocation);
        }

        //
        // GET: /RoomAllocation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClassRoomAllocation classroomallocation = db.ClassRoomAllocations.Find(id);
            if (classroomallocation == null)
            {
                return HttpNotFound();
            }
            return View(classroomallocation);
        }

        //
        // POST: /RoomAllocation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassRoomAllocation classroomallocation = db.ClassRoomAllocations.Find(id);
            db.ClassRoomAllocations.Remove(classroomallocation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewClassSchedualInformation(Department aDepartment)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");

            List<Course> courses = db.Courses.Where(c => c.DepartmentId == aDepartment.DepartmentId).ToList();
            List<ClassRoomAllocation> classRoomAllocations = db.ClassRoomAllocations.ToList();

            List<string> codes = new List<string>();
            List<string> names = new List<string>();
            List<string> scheduals = new List<string>();

            foreach (Course course in courses)
            {
                string schedual = string.Empty;
                foreach (ClassRoomAllocation classRoomAllocation in classRoomAllocations)
                {
                    int count = 0;
                    if (classRoomAllocation.CourseId == course.CourseId)
                    {
                        if (count == 0)
                        {
                            schedual += "r.no: ";
                            count++;
                        }

                        ClassRoom classRoom = (db.ClassRooms.Where(c => c.ClassRoomId == classRoomAllocation.ClassRoomId)).Single();
                        
                        schedual += (classRoom.RoomNo + "," + classRoomAllocation.Day.Name + "," +
                                        classRoomAllocation.TimeFrom + "-" + classRoomAllocation.TimeTo+";");
                    }
                    if (schedual == "r.no: ") schedual = string.Empty;
                }
                if (schedual != string.Empty)
                {
                    codes.Add(course.Code);
                    names.Add(course.Name);
                    scheduals.Add(schedual);
                }
            }

            ViewBag.CourseCodes = codes;
            ViewBag.CourseNames = names;
            ViewBag.Scheduals = scheduals;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}