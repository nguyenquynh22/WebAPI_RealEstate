using Common_BLL.Interfaces;
using Common_Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.GetUserId();

            if (userId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Token không chứa ID người dùng hợp lệ" });
            }

            var user = await _userService.GetUserByIdAsync(userId);
            return user != null ? Ok(user) : NotFound();
        }
    }
}