using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Interface
{
    public interface ILogin
    {
        Task<SqlResponse> RegisterAsync(RegisterRequestDto request);
        Task<SqlResponse> LoginAsync(LoginRequestDto request);
        string GenerateJwtToken(int userId, string email);
    }
}
