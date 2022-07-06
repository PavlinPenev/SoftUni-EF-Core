using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Homework> HomeworkSubmissions { get; set; }

        public virtual DbSet<Course> Courses { get; set; }
        
        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .IsUnicode(false)
                .IsRequired(false);

            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .IsUnicode(true);

            modelBuilder.Entity<Resource>()
                .Property(c => c.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Resource>()
                .Property(c => c.Url)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.CourseId, sc.StudentId });
        }
    }
}
