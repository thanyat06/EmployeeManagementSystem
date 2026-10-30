using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories;

public class AuditLogRepository
{
    // Add a new audit record
    public async Task<bool> AddAsync(AuditLog auditLog)
    {
        try
        {
            using var context = new AppDbContext();

            await context.AuditLogs.AddAsync(auditLog);

            await context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    // Retrieve all audit records
    public async Task<List<AuditLog>> GetAllAsync()
    {
        using var context = new AppDbContext();

        return await context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    // Retrieve an audit record by ID
    public async Task<AuditLog?> GetByIdAsync(int id)
    {
        using var context = new AppDbContext();

        return await context.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AuditLogId == id);
    }

    // Retrieve audit records by entity name
    public async Task<List<AuditLog>> GetByEntityAsync(string entityName)
    {
        using var context = new AppDbContext();

        return await context.AuditLogs
            .AsNoTracking()
            .Where(a => a.EntityName == entityName)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
}
