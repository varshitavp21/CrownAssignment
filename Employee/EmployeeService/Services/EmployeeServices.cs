using EmployeeModels.Models;
using EmployeeService.Interface;
using EmployeeService.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmployeeService.Services
{
    public class EmployeeServices : IEmployee
    {
        private readonly IDatabaseHandler _databaseHandler;
        private readonly string _connectionString;

        public EmployeeServices(IDatabaseHandler databaseHandler, IOptions<AppSetting> configuration)
        {
            _databaseHandler = databaseHandler;
            _connectionString = configuration.Value.ConnectionString.Connection;
        }

        public async Task<SqlResponse> InsertDepartment(DepartmentDto request)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_InsertDepartment", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@DepartmentName", SqlDbType.NVarChar, 300)
                    .Value = request.DepartmentName ?? (object)DBNull.Value;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapDepartmentFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.statusCode = HttpStatusCode.OK;
                    response.Message = "Department Inserted Successfully";
                }
                else
                {
                    response.Status = SqlResponseStatus.Failure;
                    response.Message = "Insert failed. No data returned.";
                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }


        public async Task<SqlResponse> UpdateDepartment(UpdateDepartmentDto request)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_UpdateDepartment", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@tblDepartmentId", SqlDbType.NVarChar, 36)
                    .Value = request.tblDepartmentId;

                command.Parameters.Add("@DepartmentName", SqlDbType.NVarChar, 300)
                    .Value = request.DepartmentName ?? (object)DBNull.Value;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapDepartmentFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.statusCode = HttpStatusCode.OK;
                    response.Message = "Department Updated Successfully";
                }
                else
                {
                    response.Status = SqlResponseStatus.Failure;
                    response.Message = "Update failed. No data returned.";
                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

        public async Task<SqlResponse> DeleteDepartment(string tblDepartmentId)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_DeleteDepartment", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@tblDepartmentId", SqlDbType.NVarChar, 36)
                    .Value = tblDepartmentId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapDepartmentFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.statusCode = HttpStatusCode.OK;
                    response.Message = "Department Deleted Successfully";
                }
                else
                {
                    response.Status = SqlResponseStatus.Failure;
                    response.Message = "Delete failed. No data returned.";
                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

        public async Task<SqlResponse> GetAllActiveDepartments()
        {
            var response = new SqlResponse();

            try
            {
                var departments = new List<Department>();

                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_GetAllActiveDepartments", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    departments.Add(MapDepartmentForGetFromReader(reader));
                }

                response.Response = departments;
                response.Status = SqlResponseStatus.Success;
                response.statusCode = HttpStatusCode.OK;

                response.Message = departments.Count > 0
                    ? "Active Departments Retrieved Successfully"
                    : "No Active Departments Found";
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

        public async Task<SqlResponse> GetDepartmentById(string tblDepartmentId)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_GetDepartmentById", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@tblDepartmentId", SqlDbType.NVarChar, 36)
                    .Value = tblDepartmentId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapDepartmentFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.statusCode = HttpStatusCode.OK;
                    response.Message = "Department Retrieved Successfully";
                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<SqlResponse> InsertEmployee(EmployeeDto request)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_InsertEmployee", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@EmployeeId", SqlDbType.NVarChar, 50)
                    .Value = request.EmployeeId ?? (object)DBNull.Value;

                command.Parameters.Add("@EmployeeName", SqlDbType.NVarChar, 200)
                    .Value = request.EmployeeName ?? (object)DBNull.Value;

                command.Parameters.Add("@DepartmentId", SqlDbType.NVarChar , 36)
                    .Value = request.DepartmentId;

                command.Parameters.Add("@Salary", SqlDbType.Int)
                    .Value = request.Salary;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapEmployeeFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.statusCode = HttpStatusCode.OK;
                    response.Message = "Employee Inserted Successfully";
                }
                else
                {
                    response.Status = SqlResponseStatus.Failure;
                    response.Message = "Insert failed. No data returned.";

                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.statusCode = HttpStatusCode.BadRequest;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

        public async Task<SqlResponse> GetAllEmployees(int pageNumber = 1, int pageSize = 10)
        {
            var response = new SqlResponse();

            try
            {
                var employeeList = new EmployeeList();
                employeeList.Employees = new List<Employee>();

                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_GetAllEmployees", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                command.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    employeeList.TotalCount = Convert.ToInt32(reader["TotalCount"]);
                }

                await reader.NextResultAsync();

                while (await reader.ReadAsync())
                {
                    employeeList.Employees.Add(MapEmployeeGetFromReader(reader));
                }

                response.Response = employeeList;
                response.Status = SqlResponseStatus.Success;
                response.statusCode = HttpStatusCode.OK;
                response.Message = employeeList.Employees.Count > 0
                    ? "Employees Retrieved Successfully"
                    : "No Employees Found";
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }

        public async Task<SqlResponse> GetEmployeeById(string employeeId)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_GetEmployeeById", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@tblEmployeeId", SqlDbType.NVarChar, 36)
                    .Value = employeeId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapEmployeeGetFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.Message = "Employee Found";
                }
                else
                {
                    response.Response = null;
                    response.Status = SqlResponseStatus.Success;
                    response.statusCode = HttpStatusCode.OK;
                    response.Message = "Employee Not Found";
                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
        public async Task<SqlResponse> UpdateEmployee(EmployeeDto request)
        {
            var response = new SqlResponse();
                
            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_UpdateEmployee", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@tblEmployeeId", SqlDbType.NVarChar, 36)
                  .Value = request.tblEmployeeId;

                command.Parameters.Add("@EmployeeId", SqlDbType.NVarChar ,50)
                    .Value = request.EmployeeId;

                command.Parameters.Add("@EmployeeName", SqlDbType.NVarChar, 200)
                    .Value = request.EmployeeName ?? (object)DBNull.Value;

                command.Parameters.Add("@DepartmentId", SqlDbType.NVarChar , 36)
                    .Value = request.DepartmentId;

                command.Parameters.Add("@Salary", SqlDbType.Int)
                    .Value = request.Salary;


                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = MapEmployeeFromReader(reader);
                    response.Status = SqlResponseStatus.Success;
                    response.Message = "Employee Updated Successfully";
                    response.statusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Status = SqlResponseStatus.Failure;
                    response.Message = "Update failed. Employee not found.";
                    response.statusCode = HttpStatusCode.OK;
                }
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
        public async Task<SqlResponse> DeleteEmployee(string tblemployeeId)
        {
            var response = new SqlResponse();

            try
            {
                using var connection =
                    (SqlConnection)await _databaseHandler.GetOpenConnectionAsync();

                using var command =
                    (SqlCommand)_databaseHandler.CreateCommand("usp_DeleteEmployee", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                command.Parameters.Add("@tblEmployeeId", SqlDbType.NVarChar, 50)
                    .Value = tblemployeeId;

                var rowsAffected = await command.ExecuteNonQueryAsync();

                response.Status = SqlResponseStatus.Success;
                response.statusCode = HttpStatusCode.OK;
                response.Message = "Employee Deleted Successfully";
            }
            catch (SqlException ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                response.Status = SqlResponseStatus.Failure;
                response.Message = $"Error: {ex.Message}";
                response.statusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }

        private EmployeeDto MapEmployeeFromReader(SqlDataReader reader)
        {
            return new EmployeeDto
            {
                //Id = reader.GetInt32(reader.GetOrdinal("Id")),
                EmployeeId = reader["EmployeeId"]?.ToString(),
                EmployeeName = reader["EmployeeName"]?.ToString(),
                DepartmentId = reader["DepartmentId"]?.ToString(),
                Salary = Convert.ToInt32(reader["Salary"]),
            };
        }


        private Employee MapEmployeeGetFromReader(SqlDataReader reader)
        {
            return new Employee
            {
                //Id = reader.GetInt32(reader.GetOrdinal("Id")),
                EmployeeId = reader["EmployeeId"]?.ToString(),
                tblEmployeeId = reader["tblEmployeeId"]?.ToString(),
                EmployeeName = reader["EmployeeName"]?.ToString(),
                DepartmentId = reader["DepartmentId"]?.ToString(),
                DepartmentName = reader["DepartmentName"]?.ToString(),
                Salary = Convert.ToInt32(reader["Salary"]),
            };
        }
        private DepartmentDto MapDepartmentFromReader(SqlDataReader reader)
        {
            return new DepartmentDto
            {
                DepartmentName = reader["DepartmentName"]?.ToString()
            };
        }

        private Department MapDepartmentForGetFromReader(SqlDataReader reader)
        {
            return new Department
            {
                DepartmentName = reader["DepartmentName"]?.ToString(),
                tblDepartmentId = reader["tblDepartmentId"]?.ToString()
            };
        }

    }
}
