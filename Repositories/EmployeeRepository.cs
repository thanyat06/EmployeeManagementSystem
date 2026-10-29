using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories;

public class EmployeeRepository
{
    public List<Employee> GetAll()
    {
        using var context = new AppDbContext();

        return context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .OrderBy(e => e.EmployeeName)
            .ToList();
    }

    public Employee? GetById(int employeeId)
    {
        using var context = new AppDbContext();

        return context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .FirstOrDefault(e => e.EmployeeId == employeeId);
    }

    public bool Add(Employee employee)
    {
        using var context = new AppDbContext();

        context.Employees.Add(employee);

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

    public bool Update(Employee employee)
    {
        using var context = new AppDbContext();

        var existingEmployee = context.Employees
            .Find(employee.EmployeeId);

        if (existingEmployee == null)
            return false;

        existingEmployee.EmployeeName = employee.EmployeeName;
        existingEmployee.Email = employee.Email;
        existingEmployee.DepartmentId = employee.DepartmentId;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.Age = employee.Age;

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

    public bool Delete(int employeeId)
    {
        using var context = new AppDbContext();

        var employee = context.Employees
            .Find(employeeId);

        if (employee == null)
            return false;

        context.Employees.Remove(employee);

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
