using Common_BLL.Interfaces;
using Common_BLL.Services;
using Common_DTOs.DTOs;
using Common_Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace AdminApi.Controllers
{

    [ApiController]
    [Route("api/contracts")]
    [AllowAnonymous] // 👈 cho test
    public class ContractsController : ControllerBase
    {
        private readonly ContractBLL _bll;

        public ContractsController(ContractBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_bll.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _bll.GetById(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Contract contract)
        {
            _bll.Create(contract);
            return Ok("Created successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Contract contract)
        {
            contract.ContractId = id;
            _bll.Update(contract);
            return Ok("Updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bll.Delete(id);
            return Ok("Deleted successfully");
        }
    }

}
