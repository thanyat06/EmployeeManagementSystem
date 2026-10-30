using System;
using System.Collections.Generic;
using System.Text;

using System.Text.Json;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _departmentRepository;
    private readonly AuditLogService _auditLogService;

    public DepartmentService()
    {
        _departmentRepository = new DepartmentRepository();
        _auditLogService = new AuditLogService();
    }

    public List<Department> GetAllDepartments()
    {
        return _departmentRepository.GetAll();
    }

    public Department? GetDepartmentById(int departmentId)
    {
        if (departmentId <= 0)
            return null;

        return _departmentRepository.GetById(departmentId);
    }

    public async Task<bool> AddDepartmentAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
            return false;

        department.DepartmentName = department.DepartmentName.Trim();

        if (department.DepartmentName.Length > 100)
            return false;

        if (department.Description?.Length > 250)
            return false;

        bool isAdded = _departmentRepository.Add(department);

        if (!isAdded)
            return false;

        string newValues = JsonSerializer.Serialize(new
        {
            department.DepartmentId,
            department.DepartmentName,
            department.Description
        });

        bool isLogged = await _auditLogService.CreateAuditLogAsync(
            "INSERT",
            "Department",
            department.DepartmentId,
            $"Added department {department.DepartmentName}",
            null,
            newValues
        );

        if (!isLogged)
            throw new InvalidOperationException(
                "Department was added, but its audit log could not be saved.");

        return true;
    }

    public async Task<bool> UpdateDepartmentAsync(Department department)
    {
        if (department.DepartmentId <= 0 ||
            string.IsNullOrWhiteSpace(department.DepartmentName))
        {
            return false;
        }

        department.DepartmentName = department.DepartmentName.Trim();

        if (department.DepartmentName.Length > 100)
            return false;

        if (department.Description?.Length > 250)
            return false;

        var existingDepartment =
            _departmentRepository.GetById(department.DepartmentId);

        if (existingDepartment == null)
            return false;

        string oldValues = JsonSerializer.Serialize(new
        {
            existingDepartment.DepartmentId,
            existingDepartment.DepartmentName,
            existingDepartment.Description
        });

        bool isUpdated = _departmentRepository.Update(department);

        if (!isUpdated)
            return false;

        string newValues = JsonSerializer.Serialize(new
        {
            department.DepartmentId,
            department.DepartmentName,
            department.Description
        });

        bool isLogged = await _auditLogService.CreateAuditLogAsync(
            "UPDATE",
            "Department",
            department.DepartmentId,
            $"Updated department {department.DepartmentName}",
            oldValues,
            newValues
        );

        if (!isLogged)
            throw new InvalidOperationException(
                "Department was updated, but its audit log could not be saved.");

        return true;
    }

    public async Task<bool> DeleteDepartmentAsync(int departmentId)
    {
        if (departmentId <= 0)
            return false;

        var existingDepartment =
            _departmentRepository.GetById(departmentId);

        if (existingDepartment == null)
            return false;

        string oldValues = JsonSerializer.Serialize(new
        {
            existingDepartment.DepartmentId,
            existingDepartment.DepartmentName,
            existingDepartment.Description
        });

        bool isDeleted = _departmentRepository.Delete(departmentId);

        if (!isDeleted)
            return false;
        
        bool isLogged = await _auditLogService.CreateAuditLogAsync(
            "DELETE",
            "Department",
            departmentId,
            $"Deleted department {existingDepartment.DepartmentName}",
            oldValues,
            null
        );

        if (!isLogged)
            throw new InvalidOperationException(
                "Department was deleted, but its audit log could not be saved.");

        return true;
    }
}

