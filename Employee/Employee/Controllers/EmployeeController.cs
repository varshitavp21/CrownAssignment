using EmployeeModels.Models;
using EmployeeService.Interface;
using EmployeeService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [ApiController]

    [Route("api/[controller]/[action]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;

        public EmployeeController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<SqlResponse> GetAllDepartment()
        {
            SqlResponse response = await _employeeService.GetAllActiveDepartments();
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpGet]
        public async Task<SqlResponse> GetDepartmentById(string tblDepartmentId)
        {
            SqlResponse response = await _employeeService.GetDepartmentById(tblDepartmentId);
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpPost]
        public async Task<SqlResponse> InsertDepartment(DepartmentDto department)
        {
            SqlResponse response = await _employeeService.InsertDepartment(department);
            response.Status = SqlResponseStatus.Success;
            return response;
        }


        [HttpPost]
        public async Task<SqlResponse> UpdateDepartment([FromBody] UpdateDepartmentDto update)
        {
            SqlResponse response = await _employeeService.UpdateDepartment(update);
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpDelete]
        public async Task<SqlResponse> DeleteDepartment(string tblDepartmentId)
        {
            SqlResponse response = await _employeeService.DeleteDepartment(tblDepartmentId);
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpGet]
        public async Task<SqlResponse> GetAllEmployee(int pageNumber =1 ,int pageSize =10)
        {
            SqlResponse response = await _employeeService.GetAllEmployees(pageNumber , pageSize);
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpGet]
        public async Task<SqlResponse> GetEmployeeById(string employeeId)
        {
            SqlResponse response = await _employeeService.GetEmployeeById(employeeId);
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpPost]
        public async Task<SqlResponse> InsertEmployee([FromBody]EmployeeDto employee)
        {
            SqlResponse response = await _employeeService.InsertEmployee(employee);
            response.Status = SqlResponseStatus.Success;
            return response;
        }


        [HttpPost]
        public async Task<SqlResponse> UpdateEmployee([FromBody] EmployeeDto employee)
        {
            SqlResponse response = await _employeeService.UpdateEmployee(employee);
            response.Status = SqlResponseStatus.Success;
            return response;
        }

        [HttpDelete]
        public async Task<SqlResponse> DeleteEmployee(string employeeId)
        {
            SqlResponse response = await _employeeService.DeleteEmployee(employeeId);
            response.Status = SqlResponseStatus.Success;
            return response;
        }
    }
}
