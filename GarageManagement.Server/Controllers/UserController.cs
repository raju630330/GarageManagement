using GarageManagement.Server.RepoInterfaces;
using GarageManagement.Server.dtos;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseAuthorizationController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        // ✅ PUT: api/user/update-role
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.RoleId <= 0)
                return BadRequest(new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid UserId or RoleId"
                });

            var result = await _userRepository.UpdateUserRoleAsync(dto.UserId, dto.RoleId);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

    }
}
