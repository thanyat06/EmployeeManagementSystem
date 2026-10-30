using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using System.Text.Json;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly DepartmentRepository _departmentRepository;
    private readonly AuditLogService _auditLogService;

    public EmployeeService()
    {
        _employeeRepository = new EmployeeRepository();
        _departmentRepository = new DepartmentRepository();
        _auditLogService = new AuditLogService();
    }

    public List<Employee> GetAllEmployees()
    {
        return _employeeRepository.GetAll();
    }

    public Employee? GetEmployeeById(int employeeId)
    {
        if (employeeId <= 0)
            return null;

        return _employeeRepository.GetById(employeeId);
    }

    // Add Employee
    public async Task<bool> AddEmployeeAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.EmployeeName) ||
            string.IsNullOrWhiteSpace(employee.Email) ||
            employee.DepartmentId <= 0 ||
            employee.Salary < 0 ||
            employee.Age <= 0)
        {
            return false;
        }

        var department =
            _departmentRepository.GetById(employee.DepartmentId);

        if (department == null)
            return false;

        bool isAdded = _employeeRepository.Add(employee);

        if (!isAdded)
            return false;

        string newValues = JsonSerializer.Serialize(new
        {
            employee.EmployeeName,
            employee.Email,
            employee.DepartmentId,
            employee.Salary,
            employee.Age
        });

        bool auditSaved = await _auditLogService.CreateAuditLogAsync(
            "INSERT",
            "Employee",
            employee.EmployeeId,
            $"Added employee {employee.EmployeeName}",
            null,
            newValues
        );

        if (!auditSaved)
            throw new InvalidOperationException(
                "Employee was added, but its audit log could not be saved.");

        return true;
    }

    // Update Employee
    public async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        if (employee.EmployeeId <= 0 ||
            string.IsNullOrWhiteSpace(employee.EmployeeName) ||
            string.IsNullOrWhiteSpace(employee.Email) ||
            employee.DepartmentId <= 0 ||
            employee.Salary < 0 ||
            employee.Age <= 0)
        {
            return false;
        }

        var department =
            _departmentRepository.GetById(employee.DepartmentId);

        if (department == null)
            return false;

        // Get existing employee before updating
        var existingEmployee =
            _employeeRepository.GetById(employee.EmployeeId);

        if (existingEmployee == null)
            return false;

        string oldValues = JsonSerializer.Serialize(new
        {
            existingEmployee.EmployeeName,
            existingEmployee.Email,
            existingEmployee.DepartmentId,
            existingEmployee.Salary,
            existingEmployee.Age
        });

        bool isUpdated = _employeeRepository.Update(employee);

        if (!isUpdated)
            return false;

        string newValues = JsonSerializer.Serialize(new
        {
            employee.EmployeeName,
            employee.Email,
            employee.DepartmentId,
            employee.Salary,
            employee.Age
        });

        bool auditSaved = await _auditLogService.CreateAuditLogAsync(
            "UPDATE",
            "Employee",
            employee.EmployeeId,
            $"Updated employee {employee.EmployeeName}",
            oldValues,
            newValues
        );

        if (!auditSaved)
            throw new InvalidOperationException(
                "Employee was updated, but its audit log could not be saved.");

        return true;
    }

    // Delete Employee
    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        if (employeeId <= 0)
            return false;

        // Get employee details before deletion
        var employee =
            _employeeRepository.GetById(employeeId);

        if (employee == null)
            return false;

        string oldValues = JsonSerializer.Serialize(new
        {
            employee.EmployeeName,
            employee.Email,
            employee.DepartmentId,
            employee.Salary,
            employee.Age
        });

        bool isDeleted = _employeeRepository.Delete(employeeId);

        if (!isDeleted)
            return false;

        bool auditSaved = await _auditLogService.CreateAuditLogAsync(
            "DELETE",
            "Employee",
            employeeId,
            $"Deleted employee {employee.EmployeeName}",
            oldValues,
            null
        );

        if (!auditSaved)
            throw new InvalidOperationException(
                "Employee was deleted, but its audit log could not be saved.");

        return true;
    }
}
