using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = GetUserIdFromToken();
            var user = await _userService.GetUserByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserUpdateRequestDto request)
        {
            var userId = GetUserIdFromToken();

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, request);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        private Guid GetUserIdFromToken()
        {
            return Guid.Parse(User.FindFirst("UserId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token."));
        }
    }
}
