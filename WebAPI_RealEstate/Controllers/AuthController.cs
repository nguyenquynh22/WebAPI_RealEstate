using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
namespace AdminApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase    
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AuthController(IUserService userService, ITokenService tokenService, IUserRepository userRepository)
        {
            _userService = userService;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var userResponseDto = await _userService.AuthenticateAsync(request.UserName, request.Password);

            if ((userResponseDto == null))
            {
                return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng" });
            }
            var userEntity =await _userRepository.GetUserByUsernameAsync(request.UserName);
            if (userEntity == null)
            {
                return Unauthorized(new { Message = "Lỗi hệ thống: Không tìm thấy hồ sơ người dùng." });
            }

            string token = _tokenService.CreateToken(userEntity);
            return Ok(new
            {
                Data = userResponseDto,
                Token = token
            });
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCreateRequestDto request)
        {
            try
            {
                var newUser = await _userService.RegisterUserAsync(request);
                return CreatedAtAction(nameof(Login), new { Data = newUser });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi trùng lặp (Username or Email already exists.)
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
