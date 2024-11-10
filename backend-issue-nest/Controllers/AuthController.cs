using backend_issue_nest.Controllers.Helper;
using backend_issue_nest.Models;
using backend_issue_nest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_issue_nest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthRepositories _authRepositories;
        public AuthController(AuthRepositories authRepositories, IConfiguration configuration)
        {
            _authRepositories = authRepositories;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest user) 
        {
            Response response = null;
            LoginResponse res = null;
            try
            {
                User loggedUser = await _authRepositories.Login(user.email, user.password);

                if (loggedUser == null)
                {
                    response = ResponseHelper.GenerateResponseData("Username or password not valid", StatusCodes.Status401Unauthorized, null, null);

                    return JSONResponse(response);
                }

                if (!loggedUser.is_active)
                {
                    response = ResponseHelper.GenerateResponseData("Your account is no more activated", StatusCodes.Status403Forbidden, null, null);

                    return JSONResponse(response);
                }

                Claim[] claims = new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id", loggedUser.id.ToString()),
                    new Claim("email", loggedUser.email),
                    new Claim("name", loggedUser.name),
                    new Claim("role", Convert.ToString(loggedUser.role)),
                    new Claim("role_name", loggedUser.role_name),
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

                res = new LoginResponse()
                {
                    id = loggedUser.id,
                    name = loggedUser.name,
                    role = loggedUser.role,
                    role_name = loggedUser.role_name,
                    email = loggedUser.email,
                    token = generatedToken,
                };

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, res, null);

                return JSONResponse(response);

            }
            catch(Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);

                return JSONResponse(response);
            }
        }

        private IActionResult JSONResponse(Response responseData)
        {
            if (responseData.status_code != 200)
            {
                return StatusCode(responseData.status_code, responseData);
            }

            return Ok(responseData);
        }
    }
}
