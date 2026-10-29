# Employee Management System

A menu-driven C# Console Application for managing employees, departments, and attendance records using Entity Framework Core and Microsoft SQL Server.

## Features

### Employee Management
- Add new employees.
- View all employees.
- Search for an employee by ID.
- Update employee details.
- Delete employee records.
- Associate employees with departments.

### Department Management
- Add, view, update, and delete departments.
- Store department names and optional descriptions.
- Prevent duplicate department names.
- Prevent deletion of departments that still have employees assigned.

### Attendance Management
- Record employee attendance.
- Store attendance dates, check-in times, and check-out times.
- Track attendance status: Present, Absent, or Leave.
- View attendance records by ID, employee, or date order.
- Update and delete attendance records.
- Prevent duplicate attendance records for the same employee on the same date.

## Technologies Used

- C#
- .NET
- Entity Framework Core
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- Visual Studio
- Git and GitHub

## Project Architecture

The application follows a layered structure:

**Menus → Services → Repositories → AppDbContext → SQL Server**

- **Models:** Represent the application's data entities.
- **Data:** Contains the EF Core database context and connection configuration.
- **Repositories:** Handle database CRUD operations.
- **Services:** Apply validation and business rules.
- **Menus:** Provide console-based interaction.
- **Migrations:** Track database schema changes.

## Database Entities

### Employee
Stores employee ID, name, email, department ID, salary, and age.

### Department
Stores department ID, department name, and an optional description.

### Attendance
Stores attendance ID, employee ID, attendance date, check-in time, check-out time, and status.

## Prerequisites

Install the following before running the project:

1. Visual Studio with the .NET desktop development workload.
2. A compatible .NET SDK.
3. Microsoft SQL Server.
4. SQL Server Management Studio (SSMS), recommended for database inspection.
5. Entity Framework Core packages compatible with the project's target framework.

## Database Configuration

Open `Data/DatabaseConfig.cs` and verify the SQL Server connection string.

The application uses a class-based connection configuration rather than a JSON settings file.

Ensure that:
- The SQL Server instance name is correct.
- Windows authentication is configured correctly.
- The database user has the required permissions.
- The database name matches the configured connection string.

## Database Setup

Open Visual Studio's **Package Manager Console** and run:

```powershell
Get-DbContext
```

To apply pending migrations:

```powershell
Update-Database
```

The project uses EF Core migrations to create and update the database schema.

## Run the Application

1. Open `EmployeeManagementSystem.sln` in Visual Studio.
2. Verify the SQL Server connection string.
3. Build the solution.
4. Ensure the database migrations have been applied.
5. Set the console application project as the startup project.
6. Run the application.

The main menu provides access to Employee Management, Department Management, Attendance Management, and Exit.

## Project Structure

```text
EmployeeManagementSystem/
├── Data/
│   ├── AppDbContext.cs
│   └── DatabaseConfig.cs
├── Models/
│   ├── Employee.cs
│   ├── Department.cs
│   └── Attendance.cs
├── Repositories/
│   ├── EmployeeRepository.cs
│   ├── DepartmentRepository.cs
│   └── AttendanceRepository.cs
├── Services/
│   ├── EmployeeService.cs
│   ├── DepartmentService.cs
│   └── AttendanceService.cs
├── Menus/
│   ├── EmployeeMenu.cs
│   ├── DepartmentMenu.cs
│   └── AttendanceMenu.cs
├── Migrations/
├── Program.cs
├── README.md
└── docs.md
```

## Learning Objectives

This project demonstrates practical use of C# classes, object-oriented programming, layered architecture, CRUD operations, Entity Framework Core, SQL Server relationships, Fluent API configuration, migrations, and Git version control.

## Future Enhancements

- Search and filter employees by department.
- Generate attendance and employee reports.
- Add stronger validation and user-friendly database error messages.
- Add authentication and role-based access.
- Introduce automated unit and integration tests.

## License

This project was created for learning and practice purposes.
