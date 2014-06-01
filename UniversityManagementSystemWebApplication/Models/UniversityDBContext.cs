using System.Data.Entity;

namespace UniversityManagementSystemWebApplication.Models
{
    public class UniversityDBContext : DbContext
    {
        public UniversityDBContext() : base("name=UniversityDBContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().HasRequired(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Teacher>().HasRequired(t => t.Designation).WithMany().HasForeignKey(t => t.DesignationId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Enrollment>().HasRequired(e => e.Course).WithMany().HasForeignKey(e => e.CourseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Enrollment>().HasRequired(e => e.Student).WithMany().HasForeignKey(e => e.StudentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClassRoomAllocation>().HasRequired(c => c.ClassRoom).WithMany().HasForeignKey(c => c.ClassRoomId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClassRoomAllocation>().HasRequired(c => c.Course).WithMany().HasForeignKey(c => c.CourseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClassRoomAllocation>().HasRequired(c => c.Department).WithMany().HasForeignKey(c => c.DepartmentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ClassRoomAllocation>().HasRequired(c => c.Day).WithMany().HasForeignKey(c => c.DayId).WillCascadeOnDelete(false);
            /*modelBuilder.Entity<ResultEntry>().HasRequired(r => r.Student).WithMany().HasForeignKey(r => r.StudentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ResultEntry>().HasRequired(r => r.Course).WithMany().HasForeignKey(r => r.CourseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ResultEntry>().HasRequired(r => r.GradeLetter).WithMany().HasForeignKey(r => r.GradeLetterId).WillCascadeOnDelete(false);*/

        }
        public DbSet<Course> Courses { set; get; }
        public DbSet<Department> Departments { set; get; }
        public DbSet<Semester> Semesters { set; get; }
        public DbSet<Designation> Designations { set; get; }
        public DbSet<Teacher> Teachers { set; get; }
        public DbSet<Student> Students { set; get; }
        public DbSet<Enrollment> Enrollments { set; get; }
        public DbSet<ClassRoom> ClassRooms { set; get; }
        public DbSet<GradeLetter> GradeLetters { set; get; }
        /*public DbSet<ResultEntry> ResultEntries { set; get; }*/
        public DbSet<Day> Days { set; get; }
        public DbSet<ClassRoomAllocation> ClassRoomAllocations { get; set; }
    }
}