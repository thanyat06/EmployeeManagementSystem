# Employee Management System — Technical Documentation

## 1. Project Overview

The Employee Management System is a C# Console Application that manages employees, departments, and attendance records. It uses Entity Framework Core (EF Core) as its Object-Relational Mapper (ORM) and Microsoft SQL Server as its relational database.

The application separates console interaction, business validation, database operations, and data models into distinct layers.

## 2. Objectives

- Implement employee, department, and attendance CRUD operations.
- Understand relationships between database entities.
- Configure EF Core using the Fluent API.
- Connect a C# application to SQL Server.
- Manage database schema changes through migrations.
- Apply repository and service separation.
- Use Git commits to track development progress.

## 3. Technologies and Tools

| Technology | Purpose |
|---|---|
| C# | Application programming language |
| .NET | Application runtime and framework |
| Entity Framework Core | ORM and database access |
| SQL Server | Relational database |
| SSMS | Database administration and query execution |
| Visual Studio | Development environment |
| Git | Version control |

## 4. Application Architecture

The application's main execution flow is:

```text
Program.cs
    |
    v
Menus
    |
    v
Services
    |
    v
Repositories
    |
    v
AppDbContext
    |
    v
SQL Server
```

### 4.1 Program.cs

Acts as the application's entry point. It displays the main menu and directs the user to the appropriate management menu.

### 4.2 Menus

The menu classes manage console input and output.

- `EmployeeMenu.cs` — employee operations.
- `DepartmentMenu.cs` — department operations.
- `AttendanceMenu.cs` — attendance operations.

### 4.3 Services

The service classes validate input, enforce basic business rules, and call the repositories.

- `EmployeeService.cs`
- `DepartmentService.cs`
- `AttendanceService.cs`

### 4.4 Repositories

The repository classes perform database operations using EF Core.

- `EmployeeRepository.cs`
- `DepartmentRepository.cs`
- `AttendanceRepository.cs`

### 4.5 Data

`AppDbContext.cs` represents the EF Core database session. It exposes entity collections through `DbSet<T>` properties and configures entity relationships.

`DatabaseConfig.cs` stores the SQL Server connection configuration in a C# class.

### 4.6 Models

The model classes represent the application's database entities.

- `Employee.cs`
- `Department.cs`
- `Attendance.cs`

### 4.7 Migrations

EF Core migrations record schema changes and allow the database schema to be updated as the models evolve.

## 5. Database Design

### 5.1 Employee Entity

| Property | Type | Purpose |
|---|---|---|
| EmployeeId | int | Primary key |
| EmployeeName | string | Employee name |
| Email | string | Employee email; unique |
| DepartmentId | int | Foreign key to Department |
| Salary | decimal | Employee salary |
| Age | int | Employee age |

### 5.2 Department Entity

| Property | Type | Purpose |
|---|---|---|
| DepartmentId | int | Primary key |
| DepartmentName | string | Department name; unique |
| Description | string? | Optional description |
| Employees | ICollection<Employee> | Related employees |

### 5.3 Attendance Entity

| Property | Type | Purpose |
|---|---|---|
| AttendanceId | int | Primary key |
| EmployeeId | int | Foreign key to Employee |
| AttendanceDate | DateOnly | Attendance date |
| CheckInTime | TimeOnly? | Optional check-in time |
| CheckOutTime | TimeOnly? | Optional check-out time |
| Status | string | Attendance status |

## 6. Entity Relationships

### 6.1 Department and Employee

One department can have multiple employees. Each employee belongs to one department through `DepartmentId`.

- Relationship: one-to-many.
- Foreign key: `Employee.DepartmentId`.
- Delete behavior: `Restrict`.
- A department with assigned employees cannot be deleted until those relationships are resolved.

### 6.2 Employee and Attendance

One employee can have multiple attendance records. Each attendance record belongs to one employee.

- Relationship: one-to-many.
- Foreign key: `Attendance.EmployeeId`.
- Delete behavior: `Cascade`.
- Deleting an employee also deletes their associated attendance records.

### 6.3 Attendance Uniqueness

A unique composite index on `EmployeeId` and `AttendanceDate` prevents an employee from having more than one attendance record for the same date.

## 7. Entity Framework Core Configuration

### 7.1 DbContext

`AppDbContext` inherits from `DbContext` and exposes:

- `DbSet<Employee> Employees`
- `DbSet<Department> Departments`
- `DbSet<Attendance> Attendances`

The context is responsible for tracking entity changes and saving them to the database.

### 7.2 Fluent API

The `OnModelCreating` method configures:

- Primary keys.
- Required fields.
- Maximum string lengths.
- Unique indexes.
- Decimal precision for salary.
- Foreign-key relationships.
- Delete behaviors.

The Fluent API centralizes database mapping rules in the context configuration.

### 7.3 Database Configuration

`DatabaseConfig.Configure()` configures the SQL Server provider and connection string.

The connection string must match the SQL Server instance installed on the development machine. Do not commit real credentials or secrets to a public repository.

## 8. CRUD Operations

CRUD stands for Create, Read, Update, and Delete.

| Operation | Purpose |
|---|---|
| Create | Add a new entity |
| Read | Retrieve one or more entities |
| Update | Modify an existing entity |
| Delete | Remove an existing entity |

Each repository provides the database operations required by its service.

## 9. Validation and Error Handling

The service layer performs basic validation before requesting database changes.

Examples include:

- Required employee and department names.
- Positive employee age and valid department IDs.
- Non-negative salary.
- Supported attendance statuses.
- Check-out time not earlier than check-in time.

Repositories handle `DbUpdateException` for database update failures and return a success or failure result.

**Limitation:** Returning only `false` does not identify the exact database error. Improved logging and user-friendly error messages can be added later.

## 10. Database Migrations

Migrations keep the database schema aligned with the EF Core model.

Run these commands in Visual Studio's Package Manager Console:

```powershell
Get-DbContext
Add-Migration MigrationName
Update-Database
```

To remove the most recent migration that has not been applied to the database:

```powershell
Remove-Migration
```

Review generated migrations before applying them, especially when changing or removing columns that may contain existing data.

## 11. Testing Checklist

- [ ] Application starts and displays the main menu.
- [ ] Department creation and listing work.
- [ ] Duplicate department names are rejected by the database.
- [ ] Employee creation requires a valid department.
- [ ] Employee details can be retrieved and updated.
- [ ] Employee deletion handles associated attendance records.
- [ ] Attendance records can be added and listed.
- [ ] Duplicate attendance for the same employee and date is rejected.
- [ ] Invalid input is handled without crashing the application.
- [ ] Database changes persist after restarting the application.

## 12. Known Limitations

- The application currently uses a console-based interface.
- Validation is basic and can be expanded.
- Database failures are not yet reported with detailed diagnostic messages.
- Authentication, authorization, reporting, and automated tests are not included in the current scope.

## 13. Future Improvements

- Add employee search, sorting, and filtering.
- Generate monthly attendance reports.
- Improve exception logging and error messages.
- Add automated tests.
- Introduce authentication and authorization.
- Add reporting and export features.

## 14. Conclusion

The project demonstrates how to build a layered C# application using EF Core and SQL Server. It provides hands-on practice with entity modeling, relational database design, Fluent API configuration, CRUD operations, service and repository responsibilities, and database migrations.
