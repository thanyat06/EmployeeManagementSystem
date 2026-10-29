using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagementSystem.Models;

public class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; }
        = new List<Employee>();
}
