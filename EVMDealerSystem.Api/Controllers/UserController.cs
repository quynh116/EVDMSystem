using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.User;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<UserResponse>>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<UserResponse>>> GetUserById(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginResponse>>> Login([FromBody] UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<LoginResponse>.Invalid("Invalid login data.", errors));
            }

            var result = await _userService.LoginAsync(request);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<UserResponse>>> UpdateUser(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<UserResponse>.Invalid("Invalid update data.", errors));
            }

            var result = await _userService.UpdateUserAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return NoContent();
            }
            return HandleResult(result);
        }

        [HttpPost("admin/Create-user")]
        public async Task<ActionResult<Result<UserResponse>>> CreateUserByAdmin([FromBody] AdminUserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<UserResponse>.Invalid("Invalid registration data.", errors));
            }


            var result = await _userService.RegisterUserByAdminAsync(request);

            if (result.ResultStatus == ResultStatus.Success && result.Data != null)
            {

                return CreatedAtAction(nameof(GetUserById), new { id = result.Data.Id }, result);
            }


            return HandleResult(result);
        }
    }
}
