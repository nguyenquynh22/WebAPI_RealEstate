using Common_BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _areaService.GetAllAreasAsync();
            return Ok(areas);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var area = await _areaService.GetAreaByIdAsync(id);
            if (area == null) return NotFound();
            return Ok(area);
        }

        // API này cực kỳ quan trọng để Frontend làm bộ lọc: 
        // Khi chọn Dự án A -> Tự động load các Phân khu của Dự án A
        [AllowAnonymous]
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            var areas = await _areaService.GetAllAreasAsync();
            var filtered = areas.Where(a => a.ProjectId == projectId).ToList();
            return Ok(filtered);
        }
    }
}