using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Common_Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminPropertyTypeController : ControllerBase
    {
        private readonly IPropertyTypeService _propertyTypeService;
        public AdminPropertyTypeController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var types = await _propertyTypeService.GetAllPropertyTypesAsync();
            return Ok(types);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await _propertyTypeService.GetPropertyTypeByIdAsync(id);
            if (type == null) return NotFound(new { message = "Không tìm thấy loại hình này" });
            return Ok(type);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertyTypeCreateRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _propertyTypeService.CreatePropertyTypeAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.PropertyTypeId }, created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PropertyTypeUpdateRequestDto request)
        {
            var updated = await _propertyTypeService.UpdatePropertyTypeAsync(id, request);
            if (updated == null) return NotFound(new { message = "Cập nhật thất bại hoặc không tìm thấy ID" });
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _propertyTypeService.DeletePropertyTypeAsync(id);
            if (!result) return NotFound(new { message = "Xóa thất bại" });
            return NoContent();
        }
    }
}
