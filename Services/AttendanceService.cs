using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services;

public class AttendanceService
{
    private readonly AttendanceRepository _attendanceRepository;
    private readonly EmployeeRepository _employeeRepository;

    public AttendanceService()
    {
        _attendanceRepository = new AttendanceRepository();
        _employeeRepository = new EmployeeRepository();
    }

    public List<Attendance> GetAllAttendance()
    {
        return _attendanceRepository.GetAll();
    }

    public Attendance? GetAttendanceById(int attendanceId)
    {
        if (attendanceId <= 0)
            return null;

        return _attendanceRepository.GetById(attendanceId);
    }

    public List<Attendance> GetAttendanceByEmployee(int employeeId)
    {
        if (employeeId <= 0)
            return new List<Attendance>();

        return _attendanceRepository.GetByEmployeeId(employeeId);
    }

    public bool AddAttendance(Attendance attendance)
    {
        if (attendance.EmployeeId <= 0 ||
            !IsValidAttendance(attendance))
        {
            return false;
        }

        var employee =
            _employeeRepository.GetById(attendance.EmployeeId);

        if (employee == null)
            return false;

        return _attendanceRepository.Add(attendance);
    }

    public bool UpdateAttendance(Attendance attendance)
    {
        if (attendance.AttendanceId <= 0 ||
            attendance.EmployeeId <= 0 ||
            !IsValidAttendance(attendance))
        {
            return false;
        }

        var employee =
            _employeeRepository.GetById(attendance.EmployeeId);

        if (employee == null)
            return false;

        return _attendanceRepository.Update(attendance);
    }

    public bool DeleteAttendance(int attendanceId)
    {
        if (attendanceId <= 0)
            return false;

        return _attendanceRepository.Delete(attendanceId);
    }

    private static bool IsValidAttendance(Attendance attendance)
    {
        if (!new[] { "Present", "Absent", "Leave" }
            .Contains(attendance.Status, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        if (attendance.CheckInTime.HasValue &&
            attendance.CheckOutTime.HasValue &&
            attendance.CheckOutTime.Value < attendance.CheckInTime.Value)
        {
            return false;
        }

        return true;
    }
}
