using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Menus;

namespace EmployeeManagementSystem;

internal class Program
{
    static async Task Main(string[] args)
    {
        var employeeMenu = new EmployeeMenu();
        var departmentMenu = new DepartmentMenu();
        var attendanceMenu = new AttendanceMenu();

        while (true)
        {
            Console.WriteLine("\n===== EMPLOYEE MANAGEMENT SYSTEM =====");
            Console.WriteLine("1. Employee Management");
            Console.WriteLine("2. Department Management");
            Console.WriteLine("3. Attendance Management");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await employeeMenu.ShowAsync();
                    break;
                case "2":
                    await departmentMenu.ShowAsync();
                    break;
                case "3":
                    attendanceMenu.Show();
                    break;
                case "0":
                    Console.WriteLine("Exiting application. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }
}
