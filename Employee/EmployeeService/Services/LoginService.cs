using Azure;
using EmployeeModels.Models;
using EmployeeService.Interface;
using EmployeeService.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeService.Services
{
    public class LoginService : ILogin
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public LoginService(IConfiguration configuration, IOptions<AppSetting> settings)
        {
            _configuration = configuration;
            _connectionString = settings.Value.ConnectionString.Connection;
        }

        public static string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes).ToLower();
        }

        public string GenerateJwtToken(int userId, string email)
        {
            var claims = new List<Claim>
        {
            new Claim("UserId", userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var secretKey = _configuration["Jwt:Secret"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToInt32(_configuration["Jwt:ExpiresIn"])
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<SqlResponse> RegisterAsync(RegisterRequestDto request)
        {
            var response = new SqlResponse();
            try
            {
                string hashedPassword = HashPassword(request.Password);

                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("usp_RegisterUser", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = request.Name;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = request.Email;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 256).Value = hashedPassword;

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    response.Response = null;
                    response.Status = SqlResponseStatus.Success;
                    response.Message = "User created successfully";
                    response.statusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Status = SqlResponseStatus.Failure;
                    response.Message = "Failed to create user.";
                    response.statusCode = HttpStatusCode.OK;
                }
                return response;
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

        public async Task<SqlResponse> LoginAsync(LoginRequestDto request)
        {
            var response = new SqlResponse();
            try
            {
                string hashedPassword = HashPassword(request.Password);

                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("usp_LoginUser", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = request.Email;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 256).Value = hashedPassword;

                await connection.OpenAsync();

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    return null;
                }

                int userId = Convert.ToInt32(reader["Id"]);
                string email = reader["Email"].ToString();

                string token = GenerateJwtToken(userId, email);

                var res = new LoginResponseDto
                {
                    Email = email,
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(
                        Convert.ToInt32(_configuration["Jwt:ExpiresIn"])
                    )
                };

                response.Response = res;
                response.Message = "Login successfull";
                response.Status = SqlResponseStatus.Success;
                response.statusCode = System.Net.HttpStatusCode.OK;
                return response;
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
    }
}