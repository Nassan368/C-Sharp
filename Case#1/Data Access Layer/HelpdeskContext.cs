using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public partial class HelpdeskContext : DbContext
    {
        public HelpdeskContext()
        {
        }

        public HelpdeskContext(DbContextOptions<HelpdeskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Problem> Problems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=HelpdeskDb;Trusted_Connection=True;");
                optionsBuilder.UseLazyLoadingProxies(); // Enables lazy loading
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Department");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timer)
                    .IsRowVersion()
                    .IsConcurrencyToken();  // Concurrency control
            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Employee");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(25)
                    .IsUnicode(false);
                entity.Property(e => e.Timer)
                    .IsRowVersion()
                    .IsConcurrencyToken();  // Concurrency control

                // Define foreign key for DepartmentId
                entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeInDept");
            });

            // Configure Problem entity
            modelBuilder.Entity<Problem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Problem");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignedEmployee).WithMany()
                    .HasForeignKey(d => d.AssignedEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProblemAssignedToEmployee");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
