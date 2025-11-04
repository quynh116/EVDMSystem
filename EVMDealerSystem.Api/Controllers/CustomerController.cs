using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.BusinessLogic.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace EVMDealerSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpGet("by-phone")]
        public async Task<IActionResult> GetByPhone([FromQuery] string phone)
        {
            var res = await _service.GetByPhoneAsync(phone);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
        {
            // dealerStaffId from JWT if present, else Guid.Empty
            var dealerStaffId = Guid.Empty;
            var res = await _service.CreateAsync(request, dealerStaffId);
            if (!res.IsSuccess) return BadRequest(res.Messages);
            return CreatedAtAction(nameof(GetById), new { id = res.Data.Id }, res.Data);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerUpdateRequest request)
        {
            var res = await _service.UpdateAsync(id, request);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _service.DeleteAsync(id);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return NoContent();
        }
    }
}
