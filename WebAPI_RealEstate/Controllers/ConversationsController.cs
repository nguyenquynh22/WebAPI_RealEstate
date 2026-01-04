using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationsService _service;

        public ConversationsController(IConversationsService service)
        {
            _service = service;
        }

        // GET: api/Conversations?userId=...&page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ConversationsFilterDto filter)
        {
            var (items, total) = await _service.GetListAsync(filter);
            return Ok(new { items, total, filter.Page, filter.PageSize });
        }

        // GET: api/Conversations/{conversationId}
        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetById(string conversationId)
        {
            var item = await _service.GetByIdAsync(conversationId);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/Conversations
        [HttpPost]
        public async Task<IActionResult> CreateOrGet([FromBody] ConversationsCreateRequestDto dto)
        {
            var id = await _service.CreateOrGetAsync(dto);
            return Ok(new { conversationId = id });
        }

        // DELETE: api/Conversations/{conversationId}
        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> Delete(string conversationId)
        {
            var ok = await _service.DeleteAsync(conversationId);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }
    }
}
