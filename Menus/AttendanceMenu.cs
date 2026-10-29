using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using System.Globalization;

namespace EmployeeManagementSystem.Menus;

public class AttendanceMenu
{
    private readonly AttendanceService _attendanceService;
    private readonly EmployeeService _employeeService;

    public AttendanceMenu()
    {
        _attendanceService = new AttendanceService();
        _employeeService = new EmployeeService();
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("\n===== ATTENDANCE MANAGEMENT =====");
            Console.WriteLine("1. View All Attendance");
            Console.WriteLine("2. View Attendance by ID");
            Console.WriteLine("3. View Attendance by Employee");
            Console.WriteLine("4. Add Attendance");
            Console.WriteLine("5. Update Attendance");
            Console.WriteLine("6. Delete Attendance");
            Console.WriteLine("0. Back");
            Console.Write("Enter your choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ViewAll();
                    break;
                case "2":
                    ViewById();
                    break;
                case "3":
                    ViewByEmployee();
                    break;
                case "4":
                    Add();
                    break;
                case "5":
                    Update();
                    break;
                case "6":
                    Delete();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private void ViewAll()
    {
        var records = _attendanceService.GetAllAttendance();

        if (records.Count == 0)
        {
            Console.WriteLine("No attendance records found.");
            return;
        }

        foreach (var record in records)
            Print(record);
    }

    private void ViewById()
    {
        if (!ReadInt("Enter attendance ID: ", out int id))
            return;

        var record = _attendanceService.GetAttendanceById(id);

        if (record == null)
        {
            Console.WriteLine("Attendance record not found.");
            return;
        }

        Print(record);
    }

    private void ViewByEmployee()
    {
        if (!ReadInt("Enter employee ID: ", out int employeeId))
            return;

        var records =
            _attendanceService.GetAttendanceByEmployee(employeeId);

        if (records.Count == 0)
        {
            Console.WriteLine("No attendance records found for that employee.");
            return;
        }

        foreach (var record in records)
            Print(record);
    }

    private void Add()
    {
        if (!ReadInt("Employee ID: ", out int employeeId))
            return;

        var employee = _employeeService.GetEmployeeById(employeeId);
        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        if (!ReadDate("Attendance date (yyyy-MM-dd): ", out DateOnly date))
            return;

        Console.Write("Check-in time (HH:mm, optional): ");
        TimeOnly? checkIn = ReadOptionalTime();

        Console.Write("Check-out time (HH:mm, optional): ");
        TimeOnly? checkOut = ReadOptionalTime();

        if (checkIn.HasValue && checkOut.HasValue &&
    checkOut.Value < checkIn.Value)
        {
            Console.WriteLine("Check-out time cannot be earlier than check-in time.");
            return;
        }

        Console.Write("Status (Present/Absent/Leave): ");
        string status = Console.ReadLine() ?? "";

        var attendance = new Attendance
        {
            EmployeeId = employeeId,
            AttendanceDate = date,
            CheckInTime = checkIn,
            CheckOutTime = checkOut,
            Status = status
        };

        Console.WriteLine(
            _attendanceService.AddAttendance(attendance)
                ? "Attendance added successfully."
                : "Could not add attendance. Check the details or duplicate date.");
    }

    private void Update()
    {
        if (!ReadInt("Attendance ID to update: ", out int id))
            return;

        var record = _attendanceService.GetAttendanceById(id);

        if (record == null)
        {
            Console.WriteLine("Attendance record not found.");
            return;
        }

        Console.Write($"Employee ID ({record.EmployeeId}): ");
        string employeeInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(employeeInput))
        {
            if (!int.TryParse(employeeInput, out int employeeId))
            {
                Console.WriteLine("Invalid employee ID.");
                return;
            }

            record.EmployeeId = employeeId;
        }

        Console.Write($"Date ({record.AttendanceDate:yyyy-MM-dd}): ");
        string dateInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            if (!DateOnly.TryParseExact(dateInput, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateOnly date))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            record.AttendanceDate = date;
        }

        Console.Write($"Check-in ({record.CheckInTime?.ToString("HH:mm") ?? "N/A"}): ");
        string checkInInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(checkInInput))
        {
            if (!TimeOnly.TryParseExact(checkInInput, "HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out TimeOnly checkIn))
            {
                Console.WriteLine("Invalid check-in time.");
                return;
            }

            record.CheckInTime = checkIn;
        }

        Console.Write($"Check-out ({record.CheckOutTime?.ToString("HH:mm") ?? "N/A"}): ");
        string checkOutInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(checkOutInput))
        {
            if (!TimeOnly.TryParseExact(checkOutInput, "HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out TimeOnly checkOut))
            {
                Console.WriteLine("Invalid check-out time.");
                return;
            }

            record.CheckOutTime = checkOut;
        }

        Console.Write($"Status ({record.Status}): ");
        string status = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(status))
            record.Status = status;

        Console.WriteLine(
            _attendanceService.UpdateAttendance(record)
                ? "Attendance updated successfully."
                : "Could not update attendance. Check the details.");
    }

    private void Delete()
    {
        if (!ReadInt("Attendance ID to delete: ", out int id))
            return;

        Console.Write("Are you sure? (y/n): ");
        if (!string.Equals(Console.ReadLine(), "y",
            StringComparison.OrdinalIgnoreCase))
            return;

        Console.WriteLine(
            _attendanceService.DeleteAttendance(id)
                ? "Attendance deleted successfully."
                : "Could not delete attendance record.");
    }

    private static void Print(Attendance record)
    {
        Console.WriteLine(
            $"Attendance ID: {record.AttendanceId} | " +
            $"Employee: {record.Employee.EmployeeName} (ID: {record.EmployeeId}) | " +
            $"Date: {record.AttendanceDate:yyyy-MM-dd} | " +
            $"Check-in: {record.CheckInTime?.ToString("HH:mm") ?? "N/A"} | " +
            $"Check-out: {record.CheckOutTime?.ToString("HH:mm") ?? "N/A"} | " +
            $"Status: {record.Status}");
    }

    private static bool ReadInt(string prompt, out int value)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out value))
            return true;

        Console.WriteLine("Please enter a valid whole number.");
        return false;
    }

    private static bool ReadDate(string prompt, out DateOnly date)
    {
        Console.Write(prompt);
        if (DateOnly.TryParseExact(Console.ReadLine(), "yyyy-MM-dd",
            CultureInfo.InvariantCulture, DateTimeStyles.None,
            out date))
            return true;

        Console.WriteLine("Please enter a date in yyyy-MM-dd format.");
        return false;
    }

    private static TimeOnly? ReadOptionalTime()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (TimeOnly.TryParseExact(input, "HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out TimeOnly time))
                return time;

            Console.Write("Invalid time. Enter HH:mm or press Enter to skip: ");
        }
    }
}
