using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Common_Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUsersController(IUserService userService)
        {
            _userService = userService;
        }

        // 1. Lấy danh sách có phân trang và lọc
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterDto filter)
        {
            var (users, totalCount) = await _userService.GetUsersAsync(filter);
            return Ok(new
            {
                Data = users,
                TotalCount = totalCount,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            });
        }

        // 2. Lấy chi tiết 1 User
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // 3. Tạo mới User (Cho phép chọn Role, mặc định Customer)
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Role)) request.Role = "Customer";
                var newUser = await _userService.RegisterUserAsync(request);
                return CreatedAtAction("GetUserById", new { id = newUser.UserId }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 4. Cập nhật thông tin Profile
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateRequestDto request)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, request);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        // 5. Cập nhật trạng thái KYC (Duyệt/Từ chối hồ sơ)
        // Đường dẫn: PUT api/admin/users/{id}/kyc
        [HttpPut("{id}/kyc")]
        public async Task<IActionResult> UpdateKyc(Guid id, [FromBody] string status)
        {
            try
            {
                var result = await _userService.UpdateKycStatusAsync(id, status);
                return Ok(result);
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        // 6. Xóa User
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}