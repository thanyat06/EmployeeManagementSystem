using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _auditLogRepository;

    public AuditLogService()
    {
        _auditLogRepository = new AuditLogRepository();
    }

    // Create a new audit record
    public async Task<bool> CreateAuditLogAsync(
        string action,
        string entityName,
        int entityId,
        string description,
        string? oldValues = null,
        string? newValues = null,
        string performedBy = "Admin")
    {
        if (string.IsNullOrWhiteSpace(action) ||
            string.IsNullOrWhiteSpace(entityName) ||
            entityId <= 0 ||
            string.IsNullOrWhiteSpace(description) ||
            string.IsNullOrWhiteSpace(performedBy))
        {
            return false;
        }

        var auditLog = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Description = description,
            OldValues = oldValues,
            NewValues = newValues,
            PerformedBy = performedBy,
            Timestamp = DateTime.Now
        };

        return await _auditLogRepository.AddAsync(auditLog);
    }

    // Retrieve all audit records
    public async Task<List<AuditLog>> GetAllAuditLogsAsync()
    {
        return await _auditLogRepository.GetAllAsync();
    }

    // Retrieve audit record by ID
    public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _auditLogRepository.GetByIdAsync(id);
    }

    // Retrieve audit records by entity name
    public async Task<List<AuditLog>> GetAuditLogsByEntityAsync(
        string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName))
            return new List<AuditLog>();

        return await _auditLogRepository.GetByEntityAsync(entityName);
    }
}
