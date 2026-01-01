using System;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesService _service;

        public PropertiesController(IPropertiesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertiesCreateRequestDto dto)
        {
            Guid id = await _service.CreateAsync(dto);
            return Ok(new { propertyId = id });
        }

        [HttpGet("{propertyId}")]
        public async Task<IActionResult> GetById(Guid propertyId)
        {
            var data = await _service.GetByIdAsync(propertyId);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PropertiesUpdateRequestDto dto)
        {
            bool ok = await _service.UpdateAsync(dto);
            if (!ok) return NotFound();
            return Ok(new { message = "Update success" });
        }

        [HttpDelete("{propertyId}")]
        public async Task<IActionResult> Delete(Guid propertyId)
        {
            bool ok = await _service.DeleteAsync(propertyId);
            if (!ok) return NotFound();
            return Ok(new { message = "Delete success" });
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PropertiesFilterDto filter)
        {
            var result = await _service.GetListAsync(filter);
            return Ok(result);
        }
    }
}
