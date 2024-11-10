using backend_issue_nest.Controllers.Helper;
using backend_issue_nest.Models;
using backend_issue_nest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_issue_nest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepositories _userRepositories;
        public UserController(UserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int? id = 0, [FromQuery] string? name = null)
        {
            Response response = null;
            try
            {
                Filter filter = new Filter()
                {
                    id = id,
                    name = name
                };

                List<User> users = await _userRepositories.GetUser(filter);

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, users, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);
                return JSONResponse(response);
            }
        }

        //[Authorize(Policy = "AdminAuthentication")]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserRequest user)
        {
            Response response = null;

            try
            {
                string res = await _userRepositories.CreateUser(user);

                if (res == "Email already exists")
                {
                    response = ResponseHelper.GenerateResponseData(res, StatusCodes.Status403Forbidden, null, null);
                    return JSONResponse(response);
                }

                response = ResponseHelper.GenerateResponseData(res, StatusCodes.Status200OK, null, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);

                return JSONResponse(response);
            }
        }

        [Authorize(Policy = "AdminAuthentication")]
        [Route("{id}/status")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserStatus([FromBody] AdminUpdateUserStatusRequest req, int id)
        {
            Response response = null;

            try
            {
                User res = await _userRepositories.UpdateUserAccountStatus(id, req.is_active);

                if (res == null)
                {
                    response = ResponseHelper.GenerateResponseData("Failed Update Data", StatusCodes.Status404NotFound, null, null);
                    return JSONResponse(response);
                }

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, res, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
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
