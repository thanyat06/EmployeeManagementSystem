using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data;

public static class DatabaseConfig
{
    private const string ConnectionString =
        @"Server=localhost\SQLEXPRESS01;Database=EmployeeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";

    public static DbContextOptions<AppDbContext> GetOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;
    }
}
