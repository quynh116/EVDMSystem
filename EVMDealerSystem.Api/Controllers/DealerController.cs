using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : BaseApiController
    {
        private readonly IDealerService _dealerService;

        public DealerController(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<DealerResponse>>>> GetAllDealers()
        {
            var result = await _dealerService.GetAllDealersAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<DealerResponse>>> GetDealerById(Guid id)
        {
            var result = await _dealerService.GetDealerByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<DealerResponse>>> CreateDealer([FromBody] DealerRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<DealerResponse>.Invalid("Invalid dealer data.", errors));
            }

            var result = await _dealerService.CreateDealerAsync(request);

            if (result.ResultStatus == ResultStatus.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetDealerById), new { id = result.Data.Id }, result);
            }

            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<DealerResponse>>> UpdateDealer(Guid id, [FromBody] DealerRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<DealerResponse>.Invalid("Invalid update data.", errors));
            }

            var result = await _dealerService.UpdateDealerAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteDealer(Guid id)
        {
            var result = await _dealerService.DeleteDealerAsync(id);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return NoContent();
            }

            return HandleResult(result);
        }
    }
}
