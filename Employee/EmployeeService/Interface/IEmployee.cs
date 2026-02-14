using EmployeeModels.Models;
using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Interface
{
    public interface IEmployee
    {
        Task<SqlResponse> InsertDepartment(DepartmentDto request);
        Task<SqlResponse> UpdateDepartment(UpdateDepartmentDto request);
        Task<SqlResponse> DeleteDepartment(string tblDepartmentId);
        Task<SqlResponse> GetAllActiveDepartments();
        Task<SqlResponse> GetDepartmentById(string tblDepartmentId);
        Task<SqlResponse> InsertEmployee(EmployeeDto request);
        Task<SqlResponse> GetAllEmployees(int pageNumber =1 ,int pageSize =10);
        Task<SqlResponse> GetEmployeeById(string employeeId);
        Task<SqlResponse> UpdateEmployee(EmployeeDto request);
        Task<SqlResponse> DeleteEmployee(string employeeId);

    }
}
