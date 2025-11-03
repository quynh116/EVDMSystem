using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.BusinessLogic.Models.Request;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EVMDealerSystem.Api.Controllers
{
    [ApiController]
    [Route("api/dealer/orders")]
    //[Authorize]
    public class DealerOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public DealerOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest request,Guid dealerStaffId)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id") ?? User.FindFirst("sub");
            //if (userIdClaim == null) return Unauthorized();

            //if (!Guid.TryParse(userIdClaim.Value, out var dealerStaffId))
            //    return Unauthorized();

            var result = await _orderService.CreateOrderAsync(request, dealerStaffId);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);

            return result.ResultStatus switch
            {
                EVMDealerSystem.BusinessLogic.Commons.ResultStatus.Invalid => BadRequest(result.Messages),
                EVMDealerSystem.BusinessLogic.Commons.ResultStatus.NotFound => NotFound(result.Messages),
                EVMDealerSystem.BusinessLogic.Commons.ResultStatus.Conflict => Conflict(result.Messages),
                EVMDealerSystem.BusinessLogic.Commons.ResultStatus.Unauthorized => Unauthorized(result.Messages),
                _ => StatusCode(500, result.Messages)
            };
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var res = await _orderService.GetByIdAsync(id);
            if (!res.IsSuccess) return NotFound(res.Messages);
            return Ok(res.Data);
        }
    }
}