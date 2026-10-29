using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagementSystem.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public int Age { get; set; }
}
