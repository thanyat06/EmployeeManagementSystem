using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagementSystem.Models;

public class AuditLog
{
    public int AuditLogId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string PerformedBy { get; set; } = "Admin";

    public DateTime Timestamp { get; set; } = DateTime.Now;
}
