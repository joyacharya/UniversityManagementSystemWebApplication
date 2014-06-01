using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace UniversityManagementSystemWebApplication.Models
{
    public class FixedData : DropCreateDatabaseIfModelChanges<UniversityDBContext>
    {
        protected override void Seed(UniversityDBContext context)
        {
            var semesters = new List<Semester>
                {
                    new Semester{Name = "1st Semester"},
                    new Semester{Name = "2nd Semester"},
                    new Semester{Name = "3rd Semester"},
                    new Semester{Name = "4th Semester"},
                    new Semester{Name = "5th Semester"},
                    new Semester{Name = "6th Semester"},
                    new Semester{Name = "7th Semester"},
                    new Semester{Name = "8th Semester"}
                };

            var designations = new List<Designation>
                {
                    new Designation{Name = "Trainer"},
                    new Designation{Name = "Department Head"},
                    new Designation{Name = "Senior Lecturer"},
                    new Designation{Name = "Lecturer"}
                };

            new List<ClassRoom>
                {
                    new ClassRoom{RoomNo = "Room-101"},
                    new ClassRoom{RoomNo = "Room-102"},
                    new ClassRoom{RoomNo = "Room-201"},
                    new ClassRoom{RoomNo = "Room-202"},
                    new ClassRoom{RoomNo = "Room-301"}
                }.ForEach(c => context.ClassRooms.Add(c));
            new List<GradeLetter>
                {
                    new GradeLetter{Name="A+"},
                    new GradeLetter{Name="A"},
                    new GradeLetter{Name="A-"},
                    new GradeLetter{Name="B+"},
                    new GradeLetter{Name="B"},
                    new GradeLetter{Name="B-"},
                    new GradeLetter{Name="C"},
                    new GradeLetter{Name="D"},
                    new GradeLetter{Name = "F"}
                }.ForEach(g =>context.GradeLetters.Add(g));

            var departments = new List<Department>
                {
                    new Department{Code = "CSE",Name = "Computer Science"},
                    new Department{Code = "EEE",Name = "Electrical Engineering "},
                    new Department{Code = "ETE",Name = "Electrical Technoly."},
                    new Department{Code = "BBA",Name = "Business Administration"}
                };

            new List<Day>
                {
                    new Day{Name = "Saturday"},
                    new Day{Name = "Sunday"},
                    new Day{Name = "Monday"},
                    new Day{Name = "Tuesday"},
                    new Day{Name = "Wednesday"},
                    new Day{Name = "Thursday"},
                    new Day{Name = "Friday"}
                }.ForEach(d => context.Days.Add(d));
            new List<Course>
                {
                    new Course{Name = "Object Oriented Programming",Code ="OOP" ,Credit = 4, Department = departments[0], Semester = semesters[0],Description = "Good and nice course"},
                    new Course{Name = "Learning Java",Code = "JAVA",Credit = 3,Department = departments[0], Semester = semesters[1],Description = "Good and nice course"},
                    new Course{Name = "Learning MySql",Code ="MySql" ,Credit = 3,Department = departments[2], Semester = semesters[2],Description = "Learn Business "},
                    new Course{Name = "Learning SQL",Code = "SQL",Credit = 2,Department = departments[1], Semester = semesters[3], Description = "Play with device"},
                    new Course{Code = ".Net",Credit = 3,Name = "Learning .Net",Description = "Advance networking good",Department=departments[1],Semester = semesters[4]},
                    new Course{Code = "C++",Credit = 3,Name = "Learning C ",Description = "Cplusplus",Department=departments[1],Semester = semesters[5]},
                    new Course{Code = "Net",Credit = 3,Name = "Networking",Description = "Networking",Department=departments[1],Semester = semesters[6]},
                    new Course{Code = "BDMS",Credit = 3,Name = "DBMS",Description = "Database",Department=departments[2],Semester = semesters[7]}
                }.ForEach(c => context.Courses.Add(c));

            new List<Teacher>
                {
                    new Teacher{Name = "Zohirul Alam Tiemoon", Address = "Dhaka", CreditToBeTaken = 10, RemainingCredit = 10,Department = departments[0], ContractNo = 01673565,Email = "km@gmail.com",Designation = designations[0] },
                    new Teacher{Name = "Taposh", Address = "Shamoli", CreditToBeTaken = 6,RemainingCredit = 6,Department = departments[0],ContractNo = 016735565,Email = "tiemoon@gmail.com",Designation = designations[1] },
                    new Teacher{Name = "Mamun", Address = "Bonani", CreditToBeTaken = 6,RemainingCredit = 6,Department = departments[2],ContractNo = 01653565,Email = "n@gmail.com",Designation = designations[2] },
                    new Teacher{Name = "Alim ", Address = "Uttara", CreditToBeTaken = 6,RemainingCredit = 6,Department = departments[3],ContractNo = 0167453565,Email = "a@gmail.com",Designation = designations[3] }
                }.ForEach( t => context.Teachers.Add(t));

            new List<Student>
                {
                    new Student{Name = "Khuku",Email = "Safrin@gmail.com",ContactNo = 64664566,Department = departments[0],RegNo = "CSE2014001",Date =Convert.ToDateTime("2013-07-15 00:00:00.000")},
                    new Student{Name = "Shahed",Email = "Shahed@gmail.com",ContactNo = 65456466,Department = departments[0],RegNo = "CSE2014002",Date =Convert.ToDateTime("2013-07-15 00:00:00.000")},
                    new Student{Name = "ShriLa",Email = "ShriLa@gmail.com",ContactNo = 768768686,Department = departments[0],RegNo = "CSE2014003",Date =Convert.ToDateTime("2013-07-17 00:00:00.000")},
                    new Student{Name = "Tanzil",Email = "Tanzil@gmail.com",ContactNo = 456674554,Department = departments[2],RegNo = "BBA2014001",Date =Convert.ToDateTime("2013-07-12 00:00:00.000")}
                }.ForEach(s => context.Students.Add(s));
        }
    }
}