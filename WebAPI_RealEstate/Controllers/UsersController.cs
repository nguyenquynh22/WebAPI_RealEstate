using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Common_Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) => _userService = userService;

        [HttpGet] 
        public async Task<IActionResult> GetAll([FromQuery] UserFilterDto filter)
        {
            var result = await _userService.GetUsersAsync(filter);
            return Ok(result);
        }

        [HttpPut("{id}/kyc")] 
        public async Task<IActionResult> UpdateKyc(Guid id, [FromBody] string status)
        {
            var result = await _userService.UpdateKycStatusAsync(id, status);
            return Ok(result);
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
