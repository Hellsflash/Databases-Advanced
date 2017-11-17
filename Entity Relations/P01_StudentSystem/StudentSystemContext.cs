using System;
using System.Net.Http.Headers;
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

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity
                    .HasKey(p => p.StudentId);

                entity
                    .Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity
                    .Property(p => p.PhoneNumber)
                    .HasColumnType("CHAR(10)")
                    .IsUnicode(false)
                    .IsRequired(false);

                entity
                    .Property(p => p.Birthday)
                    .IsRequired(false);

                entity
                    .HasMany(s => s.HomeworkSubmissions)
                    .WithOne(h => h.Student)
                    .HasForeignKey(h => h.StudentId);

                entity
                    .ToTable("Students");
            });

            modelBuilder.Entity<Course>(entity =>
            {

                entity
                    .HasKey(p => p.CourseId);

                entity
                    .Property(p => p.Name)
                    .HasMaxLength(80)
                    .IsUnicode(true);

                entity
                    .Property(p => p.Description)
                    .IsUnicode(true)
                    .IsRequired(false);

                entity
                    .HasMany(c => c.Resources)
                    .WithOne(r => r.Course)
                    .HasForeignKey(r => r.CourseId);

                entity
                    .HasMany(c => c.HomeworkSubmissions)
                    .WithOne(hm => hm.Course)
                    .HasForeignKey(hm => hm.CourseId);

                entity
                    .ToTable("Courses");
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity
                    .HasKey(p => p.ResourceId);

                entity
                    .Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsUnicode(true);

                entity
                    .Property(p => p.Url)
                    .IsUnicode(false);

                entity
                    .ToTable("Resources");
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity
                    .HasKey(p => p.HomeworkId);

                entity
                    .Property(p => p.Content)
                    .IsUnicode(false);

                entity
                    .ToTable("HomeworkSubmissions");
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity
                    .HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity
                    .HasOne(sc => sc.Student)
                    .WithMany(s => s.CourseEnrollments)
                    .HasForeignKey(sc => sc.StudentId);


                entity
                    .HasOne(sc => sc.Course)
                    .WithMany(s => s.StudentsEnrolled)
                    .HasForeignKey(sc => sc.CourseId);
                   

                entity
                    .ToTable("StudentCourses");
            });


        }
    }
}
