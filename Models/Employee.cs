using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagementSystem.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public decimal Salary { get; set; }

    public int Age { get; set; }

    public Department Department { get; set; } = null!;

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();
}

