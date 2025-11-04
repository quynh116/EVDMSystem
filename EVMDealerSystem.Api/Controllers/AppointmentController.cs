using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.BusinessLogic.Models.Request;

namespace EVMDealerSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;
        public AppointmentController(IAppointmentService service) { _service = service; }

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

        [HttpGet("dealer/{dealerId:guid}")]
        public async Task<IActionResult> GetByDealer(Guid dealerId)
        {
            var res = await _service.GetByDealerIdAsync(dealerId);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpGet("vehicle-date")]
        public async Task<IActionResult> GetByVehicleDate([FromQuery] Guid vehicleId, [FromQuery] DateTime date)
        {
            var res = await _service.GetByVehicleDateAsync(vehicleId, date);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] Guid vehicleId, [FromQuery] DateTime date)
        {
            var res = await _service.GetAvailableSlotsAsync(vehicleId, date);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateRequest request)
        {
            var userClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();
            if (!Guid.TryParse(userClaim.Value, out var dealerStaffId)) return Unauthorized();

            var res = await _service.CreateAsync(request, dealerStaffId);
            if (!res.IsSuccess) return BadRequest(res.Messages);
            return CreatedAtAction(nameof(GetById), new { id = res.Data.Id }, res.Data);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentUpdateRequest request)
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
