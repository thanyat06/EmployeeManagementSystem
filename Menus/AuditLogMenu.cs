using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Menus;

public class AuditLogMenu
{
    private readonly AuditLogService _auditLogService;

    public AuditLogMenu()
    {
        _auditLogService = new AuditLogService();
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== AUDIT LOG MANAGEMENT =====");
            Console.WriteLine("1. View All Audit Logs");
            Console.WriteLine("2. View Audit Log by ID");
            Console.WriteLine("3. Filter Audit Logs by Entity");
            Console.WriteLine("0. Back");
            Console.Write("Enter your choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await ViewAllAsync();
                    break;
                case "2":
                    await ViewByIdAsync();
                    break;
                case "3":
                    await ViewByEntityAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private async Task ViewAllAsync()
    {
        var auditLogs = await _auditLogService.GetAllAuditLogsAsync();

        if (auditLogs.Count == 0)
        {
            Console.WriteLine("No audit logs found.");
            return;
        }

        foreach (var auditLog in auditLogs)
        {
            DisplayAuditLog(auditLog);
        }
    }

    private async Task ViewByIdAsync()
    {
        if (!ReadInt("Enter audit log ID: ", out int id))
            return;

        var auditLog =
            await _auditLogService.GetAuditLogByIdAsync(id);

        if (auditLog == null)
        {
            Console.WriteLine("Audit log not found.");
            return;
        }

        DisplayAuditLog(auditLog);
    }

    private async Task ViewByEntityAsync()
    {
        Console.Write("Enter entity name (Employee/Department): ");
        string entityName = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Console.WriteLine("Entity name cannot be empty.");
            return;
        }

        var auditLogs =
            await _auditLogService.GetAuditLogsByEntityAsync(entityName.Trim());

        if (auditLogs.Count == 0)
        {
            Console.WriteLine("No audit logs found for this entity.");
            return;
        }

        foreach (var auditLog in auditLogs)
        {
            DisplayAuditLog(auditLog);
        }
    }

    private static void DisplayAuditLog(AuditLog auditLog)
    {
        Console.WriteLine("\n------------------------------------");
        Console.WriteLine($"Audit Log ID : {auditLog.AuditLogId}");
        Console.WriteLine($"Action       : {auditLog.Action}");
        Console.WriteLine($"Entity       : {auditLog.EntityName}");
        Console.WriteLine($"Entity ID    : {auditLog.EntityId}");
        Console.WriteLine($"Description  : {auditLog.Description}");
        Console.WriteLine($"Old Values   : {auditLog.OldValues ?? "N/A"}");
        Console.WriteLine($"New Values   : {auditLog.NewValues ?? "N/A"}");
        Console.WriteLine($"Performed By : {auditLog.PerformedBy}");
        Console.WriteLine($"Timestamp    : {auditLog.Timestamp}");
        Console.WriteLine("------------------------------------");
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
