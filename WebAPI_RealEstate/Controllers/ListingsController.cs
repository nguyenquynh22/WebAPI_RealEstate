using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DTOs.DTOs;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IListingsService _service;

        public ListingsController(IListingsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListingsCreateRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { listingId = id });
        }

        [HttpGet("{listingId}")]
        public async Task<IActionResult> GetById(Guid listingId)
        {
            var item = await _service.GetByIdAsync(listingId);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ListingsUpdateRequestDto dto)
        {
            var ok = await _service.UpdateAsync(dto);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }

        [HttpDelete("{listingId}")]
        public async Task<IActionResult> Delete(Guid listingId)
        {
            var ok = await _service.DeleteAsync(listingId);
            if (!ok) return NotFound();
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ListingsFilterDto filter)
        {
            var (items, total) = await _service.GetListAsync(filter);
            return Ok(new { items, total, page = filter.Page, pageSize = filter.PageSize });
        }
    }
}
