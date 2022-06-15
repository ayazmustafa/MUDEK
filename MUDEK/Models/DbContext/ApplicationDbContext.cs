using Microsoft.EntityFrameworkCore;

namespace Mudek.Models
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Course> Courses{ get; set; }
        public virtual DbSet<Department> Departments{ get; set; }
        public virtual DbSet<DepartmentCourse> DepartmentCourses { get; set; }
        public virtual DbSet<DepartmentTeacher> DepartmentTeachers { get; set; }
        public virtual DbSet<CourseOutcome> CourseOutcomes { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<OpenedCourse> OpenedCourses { get; set; }
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentOpenedCourse> StudentOpenedCourses { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<DepartmentOutcomes> DepartmentOutcomes { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseMySql("server=localhost;port=3306;user=root;password=asd123123;database=Mudek", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentOpenedCourse>()
                .HasOne(x => x.Student)
                .WithMany(t => t.StudentOpenedCourses)
                .HasForeignKey(t => t.StudentId)
                .IsRequired();

            modelBuilder.Entity<StudentOpenedCourse>()
                .HasOne(x => x.Course)
                .WithMany(t => t.StudentOpenedCourses)
                .HasForeignKey(t => t.OpenedCourseId)
                .IsRequired();


            modelBuilder.Entity<DepartmentCourse>()
                .HasOne(x => x.Course)
                .WithMany(t => t.DepartmentCourses)
                .HasForeignKey(t => t.CourseId)
                .IsRequired();

            modelBuilder.Entity<DepartmentCourse>()
                .HasOne(x => x.Department)
                .WithMany(t => t.DepartmentCourses)
                .HasForeignKey(t => t.DepartmentId)
                .IsRequired();

        }
    }
}
