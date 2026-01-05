using Microsoft.AspNetCore.Mvc;
using REstate.DAL;
using REstate.DTO;

namespace REstate.API.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractsController : ControllerBase
    {
        private readonly ContractDAL _dal;

        public ContractsController(ContractDAL dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dal.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _dal.GetByIdAsync(id);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(dto_ConTract dto)
        {
            return await _dal.CreateAsync(dto)
                ? Ok("Created")
                : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, dto_ConTract dto)
        {
            return await _dal.UpdateAsync(id, dto)
                ? Ok("Updated")
                : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _dal.DeleteAsync(id)
                ? Ok("Deleted")
                : NotFound();
        }
    }
}
