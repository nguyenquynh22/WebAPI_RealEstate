using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _service;

        public CommentsController(ICommentsService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CommentsFilterDto filter)
        {
            var (items, total) = await _service.GetListAsync(filter);
            return Ok(new { items, total, filter.Page, filter.PageSize });
        }

        // GET: api/Comments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentsCreateRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { commentId = id });
        }

        // PUT: api/Comments
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CommentsUpdateRequestDto dto)
        {
            var ok = await _service.UpdateAsync(dto);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }
    }
}
