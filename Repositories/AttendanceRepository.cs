using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories;

public class AttendanceRepository
{
    public List<Attendance> GetAll()
    {
        using var context = new AppDbContext();

        return context.Attendances
            .AsNoTracking()
            .Include(a => a.Employee)
            .OrderByDescending(a => a.AttendanceDate)
            .ToList();
    }

    public Attendance? GetById(int attendanceId)
    {
        using var context = new AppDbContext();

        return context.Attendances
            .AsNoTracking()
            .Include(a => a.Employee)
            .FirstOrDefault(a => a.AttendanceId == attendanceId);
    }

    public List<Attendance> GetByEmployeeId(int employeeId)
    {
        using var context = new AppDbContext();

        return context.Attendances
            .AsNoTracking()
            .Include(a => a.Employee)
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.AttendanceDate)
            .ToList();
    }

    public bool Add(Attendance attendance)
    {
        using var context = new AppDbContext();

        context.Attendances.Add(attendance);

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

    public bool Update(Attendance attendance)
    {
        using var context = new AppDbContext();

        var existingAttendance = context.Attendances
            .Find(attendance.AttendanceId);

        if (existingAttendance == null)
            return false;

        existingAttendance.EmployeeId = attendance.EmployeeId;
        existingAttendance.AttendanceDate = attendance.AttendanceDate;
        existingAttendance.CheckInTime = attendance.CheckInTime;
        existingAttendance.CheckOutTime = attendance.CheckOutTime;
        existingAttendance.Status = attendance.Status;

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

    public bool Delete(int attendanceId)
    {
        using var context = new AppDbContext();

        var attendance = context.Attendances
            .Find(attendanceId);

        if (attendance == null)
            return false;

        context.Attendances.Remove(attendance);

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
