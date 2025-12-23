using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userService.AuthenticateAsync(request.UserName, request.Password);
            if (user == null) return Unauthorized("Sai tài khoản hoặc mật khẩu");

            // Tạo Token chứa thông tin User
            var token = _tokenService.CreateToken(user);
            return Ok(new { Token = token, User = user });
        }
    }
}