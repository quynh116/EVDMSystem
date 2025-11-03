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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;
        public FeedbackController(IFeedbackService service) { _service = service; }

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

        [HttpGet("by-order/{orderId:guid}")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var res = await _service.GetByOrderIdAsync(orderId);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackCreateRequest request)
        {
            // customer id from JWT claim (assuming customer posts feedback) or other flow
            var userClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();
            if (!Guid.TryParse(userClaim.Value, out var customerId)) return Unauthorized();

            var res = await _service.CreateAsync(request, customerId);
            if (!res.IsSuccess) return BadRequest(res.Messages);
            return CreatedAtAction(nameof(GetById), new { id = res.Data.Id }, res.Data);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] FeedbackUpdateRequest request)
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
