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
    public class AdminAreaController : ControllerBase 
    {
        private readonly IAreaService _areaService;

        public AdminAreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await _areaService.GetAllAreasAsync();
            return Ok(areas);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetAreaById(Guid id) 
        {
            var area = await _areaService.GetAreaByIdAsync(id);
            if (area == null) return NotFound(new { message = "Không tìm thấy khu vực" });

            return Ok(area);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArea([FromBody] AreaCreateRequestDto request)
        {
            try
            {
                var createdArea = await _areaService.CreateAreaAsync(request);
                // Trả về 201 Created và link lấy lại object vừa tạo
                return CreatedAtAction(nameof(GetAreaById), new { id = createdArea.AreaId }, createdArea);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message }); // Lỗi nếu ProjectId không tồn tại
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(Guid id, [FromBody] AreaUpdateRequestDto request)
        {
            if (id != request.AreaId) return BadRequest(new { message = "ID không khớp" });

            try
            {
                var updatedArea = await _areaService.UpdateAreaAsync(request);
                return Ok(updatedArea);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(Guid id)
        {
            var result = await _areaService.DeleteAreaAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}