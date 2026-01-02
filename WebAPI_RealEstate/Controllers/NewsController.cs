using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsCreateRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { newsId = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NewsUpdateRequestDto dto)
        {
            var ok = await _service.UpdateAsync(dto);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }

        [HttpDelete("{newsId}")]
        public async Task<IActionResult> Delete(Guid newsId)
        {
            var ok = await _service.DeleteAsync(newsId);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }
    }
}
