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

    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

            entity.Property(e => e.Department)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Salary)
                .HasPrecision(18, 2);

            entity.Property(e => e.Age)
                .IsRequired();
        });
    }
}
