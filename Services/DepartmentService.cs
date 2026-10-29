using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _departmentRepository;

    public DepartmentService()
    {
        _departmentRepository = new DepartmentRepository();
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

    public bool AddDepartment(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
            return false;

        department.DepartmentName = department.DepartmentName.Trim();

        if (department.DepartmentName.Length > 100)
            return false;

        if (department.Description?.Length > 250)
            return false;

        return _departmentRepository.Add(department);
    }

    public bool UpdateDepartment(Department department)
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

        return _departmentRepository.Update(department);
    }

    public bool DeleteDepartment(int departmentId)
    {
        if (departmentId <= 0)
            return false;

        return _departmentRepository.Delete(departmentId);
    }
}
