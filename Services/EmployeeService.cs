using System;
using System.Collections.Generic;
using System.Text;
using EmployeeManagementSystem.Data;

using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly DepartmentRepository _departmentRepository;

    public EmployeeService()
    {
        _employeeRepository = new EmployeeRepository();
        _departmentRepository = new DepartmentRepository();
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

    public bool AddEmployee(Employee employee)
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

        return _employeeRepository.Add(employee);
    }

    public bool UpdateEmployee(Employee employee)
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

        return _employeeRepository.Update(employee);
    }

    public bool DeleteEmployee(int employeeId)
    {
        if (employeeId <= 0)
            return false;

        return _employeeRepository.Delete(employeeId);
    }
}
