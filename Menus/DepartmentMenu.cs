using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Menus;

public class DepartmentMenu
{
    private readonly DepartmentService _departmentService;

    public DepartmentMenu()
    {
        _departmentService = new DepartmentService();
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== DEPARTMENT MANAGEMENT =====");
            Console.WriteLine("1. View All Departments");
            Console.WriteLine("2. View Department by ID");
            Console.WriteLine("3. Add Department");
            Console.WriteLine("4. Update Department");
            Console.WriteLine("5. Delete Department");
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
        var departments = _departmentService.GetAllDepartments();

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments found.");
            return;
        }

        foreach (var department in departments)
        {
            Console.WriteLine(
                $"ID: {department.DepartmentId} | " +
                $"Name: {department.DepartmentName} | " +
                $"Description: {department.Description ?? "N/A"}");
        }
    }

    private void ViewById()
    {
        if (!ReadInt("Enter department ID: ", out int id))
            return;

        var department = _departmentService.GetDepartmentById(id);

        if (department == null)
        {
            Console.WriteLine("Department not found.");
            return;
        }

        Console.WriteLine(
            $"ID: {department.DepartmentId} | " +
            $"Name: {department.DepartmentName} | " +
            $"Description: {department.Description ?? "N/A"}");
    }

    private async Task AddAsync()
    {
        Console.Write("Department name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Description (optional): ");
        string description = Console.ReadLine() ?? "";

        var department = new Department
        {
            DepartmentName = name,
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description
        };

        try
        {
            bool result =
                await _departmentService.AddDepartmentAsync(department);

            Console.WriteLine(
                result
                    ? "Department added successfully."
                    : "Could not add department. Check the details or duplicate name.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task UpdateAsync()
    {
        if (!ReadInt("Enter department ID to update: ", out int id))
            return;

        var department = _departmentService.GetDepartmentById(id);

        if (department == null)
        {
            Console.WriteLine("Department not found.");
            return;
        }

        Console.Write($"Name ({department.DepartmentName}): ");
        string name = Console.ReadLine() ?? "";

        if (!string.IsNullOrWhiteSpace(name))
            department.DepartmentName = name;

        Console.Write($"Description ({department.Description ?? "N/A"}): ");
        string description = Console.ReadLine() ?? "";

        if (!string.IsNullOrWhiteSpace(description))
            department.Description = description;

        try
        {
            bool result =
                await _departmentService.UpdateDepartmentAsync(department);

            Console.WriteLine(
                result
                    ? "Department updated successfully."
                    : "Could not update department. Check the details.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task DeleteAsync()
    {
        if (!ReadInt("Enter department ID to delete: ", out int id))
            return;

        Console.Write("Are you sure? (y/n): ");

        if (!string.Equals(Console.ReadLine(), "y",
            StringComparison.OrdinalIgnoreCase))
            return;

        try
        {
            bool result =
                await _departmentService.DeleteDepartmentAsync(id);

            Console.WriteLine(
                result
                    ? "Department deleted successfully."
                    : "Could not delete department. It may not exist or may have employees assigned.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static bool ReadInt(string prompt, out int value)
    {
        Console.Write(prompt);

        if (int.TryParse(Console.ReadLine(), out value))
            return true;

        Console.WriteLine("Please enter a valid whole number.");
        return false;
    }
}
