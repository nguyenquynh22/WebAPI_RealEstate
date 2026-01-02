using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Common_Shared.Constants; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdminApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/admin/[controller]")] 
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] NewsFilterDto filter)
        {
            var (items, total) = await _service.GetListAsync(filter);
            return Ok(new { items, total, filter.Page, filter.PageSize });
        }

        [HttpGet("{newsId}")]
        public async Task<IActionResult> GetById(Guid newsId)
        {
            var item = await _service.GetByIdAsync(newsId);
            if (item == null) return NotFound(new { message = "Không tìm thấy tin tức này." });
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsCreateRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { newsId = id }, new { newsId = id });
        }

        [HttpPut("{newsId}")]
        public async Task<IActionResult> Update(Guid newsId, [FromBody] NewsUpdateRequestDto dto)
        {
            if (newsId != dto.NewsId) return BadRequest(new { message = "ID không khớp." });

            var ok = await _service.UpdateAsync(dto);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }

        [HttpDelete("{newsId}")]
        public async Task<IActionResult> Delete(Guid newsId)
        {
            var ok = await _service.DeleteAsync(newsId);
            if (!ok) return NotFound();
            return NoContent(); 
        }
    }
}