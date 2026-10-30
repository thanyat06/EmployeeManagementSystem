using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagementSystem.Models;

public class Attendance
{
    public int AttendanceId { get; set; }
    //Foreign Key
    public int EmployeeId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public string Status { get; set; } = "Present";

    public Employee Employee { get; set; } = null!;
}
