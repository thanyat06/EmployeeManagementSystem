using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories;

public class DepartmentRepository
{
    public List<Department> GetAll()
    {
        using var context = new AppDbContext();

        return context.Departments
            .AsNoTracking()
            .OrderBy(d => d.DepartmentName)
            .ToList();
    }

    public Department? GetById(int departmentId)
    {
        using var context = new AppDbContext();

        return context.Departments
            .AsNoTracking()
            .FirstOrDefault(d => d.DepartmentId == departmentId);
    }

    public bool Add(Department department)
    {
        using var context = new AppDbContext();

        context.Departments.Add(department);

        try
        {
            context.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public bool Update(Department department)
    {
        using var context = new AppDbContext();

        var existingDepartment = context.Departments
            .Find(department.DepartmentId);

        if (existingDepartment == null)
            return false;

        existingDepartment.DepartmentName = department.DepartmentName;
        existingDepartment.Description = department.Description;

        try
        {
            context.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public bool Delete(int departmentId)
    {
        using var context = new AppDbContext();

        var department = context.Departments
            .Find(departmentId);

        if (department == null)
            return false;

        context.Departments.Remove(department);

        try
        {
            context.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
}
