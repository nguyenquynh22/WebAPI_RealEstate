using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")] 
[Route("api/admin/users")]
[ApiController]
public class AdminUsersController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminUsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/admin/users?pageIndex=1&pageSize=10&keyword=...
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilterDto filter)
    {
        var (users, totalCount) = await _userService.GetUsersAsync(filter);
        return Ok(new { Data = users, TotalCount = totalCount, PageIndex = filter.PageIndex, PageSize = filter.PageSize });
    }

    // GET: api/admin/users/{id}
    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // PUT: api/admin/users/{id} (Admin c?p nh?t profile ng??i khác)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateRequestDto request)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(id, request);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // PATCH: api/admin/users/{id}/kyc (Admin c?p nh?t tr?ng thái KYC)
    [HttpPatch("{id}/kyc")]
    public async Task<IActionResult> UpdateKyc(Guid id, [FromQuery] string status)
    {
        try
        {
            var updatedUser = await _userService.UpdateKycStatusAsync(id, status);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // DELETE: api/admin/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto request)
    {
        try
        {
            // LOGIC MỚI: Nếu Role bị bỏ trống (null), mặc định là "Customer"
            if (string.IsNullOrWhiteSpace(request.Role))
            {
                request.Role = "Customer";
            }
            // Admin API cho phép tạo bất kỳ Role nào được truyền vào

            var newUser = await _userService.RegisterUserAsync(request);

            // Trả về 201 Created và gọi phương thức GetUserById
            return CreatedAtAction("GetUserById", new { id = newUser.UserId }, newUser);
        }
        catch (Exception ex)
        {
            // Xử lý lỗi trùng lặp từ UserService
            return BadRequest(new { Message = ex.Message });
        }
    }
}