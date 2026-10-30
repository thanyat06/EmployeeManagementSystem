using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Menus;

public class EmployeeMenu
{
    private readonly EmployeeService _employeeService;
    private readonly DepartmentService _departmentService;

    public EmployeeMenu()
    {
        _employeeService = new EmployeeService();
        _departmentService = new DepartmentService();
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== EMPLOYEE MANAGEMENT =====");
            Console.WriteLine("1. View All Employees");
            Console.WriteLine("2. View Employee by ID");
            Console.WriteLine("3. Add Employee");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
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
                    await AddAsync();
                    break;
                case "4":
                    await UpdateAsync();
                    break;
                case "5":
                    await DeleteAsync();
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
        var employees = _employeeService.GetAllEmployees();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (var employee in employees)
        {
            Console.WriteLine(
                $"ID: {employee.EmployeeId} | " +
                $"Name: {employee.EmployeeName} | " +
                $"Email: {employee.Email} | " +
                $"Department: {employee.Department.DepartmentName} | " +
                $"Salary: {employee.Salary:F2} | Age: {employee.Age}");
        }
    }

    private void ViewById()
    {
        if (!ReadInt("Enter employee ID: ", out int id))
            return;

        var employee = _employeeService.GetEmployeeById(id);

        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Console.WriteLine(
            $"ID: {employee.EmployeeId} | Name: {employee.EmployeeName} | " +
            $"Email: {employee.Email} | " +
            $"Department: {employee.Department.DepartmentName} | " +
            $"Salary: {employee.Salary:F2} | Age: {employee.Age}");
    }

    private async Task AddAsync()
    {
        Console.Write("Employee name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        if (!ReadInt("Department ID: ", out int departmentId) ||
            !ReadDecimal("Salary: ", out decimal salary) ||
            !ReadInt("Age: ", out int age))
            return;

        var employee = new Employee
        {
            EmployeeName = name,
            Email = email,
            DepartmentId = departmentId,
            Salary = salary,
            Age = age
        };

        try
        {
            bool result = await _employeeService.AddEmployeeAsync(employee);

            Console.WriteLine(
                result
                    ? "Employee added successfully."
                    : "Could not add employee. Check the details, department ID, or duplicate email.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task UpdateAsync()
    {
        if (!ReadInt("Enter employee ID to update: ", out int id))
            return;

        var existing = _employeeService.GetEmployeeById(id);

        if (existing == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Console.Write($"Name ({existing.EmployeeName}): ");
        string name = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(name))
            existing.EmployeeName = name;

        Console.Write($"Email ({existing.Email}): ");
        string email = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(email))
            existing.Email = email;

        Console.Write($"Department ID ({existing.DepartmentId}): ");
        string departmentInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(departmentInput))
        {
            if (!int.TryParse(departmentInput, out int departmentId))
            {
                Console.WriteLine("Invalid department ID.");
                return;
            }

            existing.DepartmentId = departmentId;
        }

        Console.Write($"Salary ({existing.Salary}): ");
        string salaryInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(salaryInput))
        {
            if (!decimal.TryParse(salaryInput, out decimal salary))
            {
                Console.WriteLine("Invalid salary.");
                return;
            }

            existing.Salary = salary;
        }

        Console.Write($"Age ({existing.Age}): ");
        string ageInput = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(ageInput))
        {
            if (!int.TryParse(ageInput, out int age))
            {
                Console.WriteLine("Invalid age.");
                return;
            }

            existing.Age = age;
        }

        try
        {
            bool result = await _employeeService.UpdateEmployeeAsync(existing);

            Console.WriteLine(
                result
                    ? "Employee updated successfully."
                    : "Could not update employee. Check the details.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task DeleteAsync()
    {
        if (!ReadInt("Enter employee ID to delete: ", out int id))
            return;

        Console.Write("Are you sure? (y/n): ");
        if (!string.Equals(Console.ReadLine(), "y",
            StringComparison.OrdinalIgnoreCase))
            return;

        try
        {
            bool result = await _employeeService.DeleteEmployeeAsync(id);

            Console.WriteLine(
                result
                    ? "Employee deleted successfully."
                    : "Could not delete employee.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static bool ReadInt(string prompt, out int value)
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out value))
                return true;

            Console.WriteLine("Please enter a valid whole number.");
        }
    }

    private static bool ReadDecimal(string prompt, out decimal value)
    {
        while (true)
        {
            Console.Write(prompt);

            if (decimal.TryParse(Console.ReadLine(), out value))
                return true;

            Console.WriteLine("Please enter a valid amount.");
        }
    }
}
