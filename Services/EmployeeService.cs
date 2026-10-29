using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Services;

public class EmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public List<Employee> GetAllEmployees()
    {
        return _context.Employees
            .AsNoTracking()
            .ToList();
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        return _context.Employees
            .FirstOrDefault(e => e.EmployeeId == employeeId);
    }
}
