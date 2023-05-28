using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace draft;

public partial class DbuniversityContext : DbContext
{
    public DbuniversityContext()
    {
    }

    public DbuniversityContext(DbContextOptions<DbuniversityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseAssignment> CourseAssignments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Studying> Studyings { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= WIN-PQ468N1423B\\SQLEXPRESS; Database=DBUniversity;  Trusted_Connection=True;  Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Courses)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Department");
        });

        modelBuilder.Entity<CourseAssignment>(entity =>
        {
            entity.ToTable("CourseAssignment");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseAssignments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseAssignment_Course");

            entity.HasOne(d => d.Teacher).WithMany(p => p.CourseAssignments)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseAssignment_Teacher");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.EnrollmentDate).HasColumnType("date");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Studying>(entity =>
        {
            entity.ToTable("Studying");

            entity.HasOne(d => d.Course).WithMany(p => p.Studyings)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Studying_Courses");

            entity.HasOne(d => d.Student).WithMany(p => p.Studyings)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Studying_Students");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.HireDate).HasColumnType("date");
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Teacher_Departments");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
