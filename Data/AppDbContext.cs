using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            DatabaseConfig.Configure(optionsBuilder);
        }
    }

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Department> Departments { get; set; } = null!;

    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.DepartmentId);

            entity.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(d => d.DepartmentName)
                .IsUnique();

            entity.Property(d => d.Description)
                .HasMaxLength(250);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.Salary)
                .HasPrecision(18, 2);

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(a => a.AttendanceId);

            entity.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(a => new
            {
                a.EmployeeId,
                a.AttendanceDate
            })
                .IsUnique();

            entity.HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
