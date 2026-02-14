using EmployeeService.Interface;
using EmployeeService.Models;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [ApiController]

    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class LoginController : ControllerBase
    {
        public ILogin _loginService;
        public LoginController(ILogin loginService)
        {
            _loginService = loginService;
        }


        [HttpPost("register")]
        public async Task<SqlResponse> Register([FromBody]RegisterRequestDto request)
        {
            var result = await _loginService.RegisterAsync(request);
            return result;
        }

        [HttpPost]
        public async Task<SqlResponse> Login(LoginRequestDto request)
        {
            var response = await _loginService.LoginAsync(request);

            return response;
        }
    }
}
