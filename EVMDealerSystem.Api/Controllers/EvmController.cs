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
    public class EvmController : BaseApiController
    {
        private readonly IEvmService _evmService;

        public EvmController(IEvmService evmService)
        {
            _evmService = evmService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<EvmResponse>>>> GetAllEvms()
        {
            var result = await _evmService.GetAllEvmsAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<EvmResponse>>> GetEvmById(Guid id)
        {
            var result = await _evmService.GetEvmByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<EvmResponse>>> CreateEvm([FromBody] EvmRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<EvmResponse>.Invalid("Invalid EVM data.", errors));
            }

            var result = await _evmService.CreateEvmAsync(request);

            if (result.ResultStatus == ResultStatus.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetEvmById), new { id = result.Data.Id }, result);
            }

            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<EvmResponse>>> UpdateEvm(Guid id, [FromBody] EvmRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<EvmResponse>.Invalid("Invalid update data.", errors));
            }

            var result = await _evmService.UpdateEvmAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteEvm(Guid id)
        {
            var result = await _evmService.DeleteEvmAsync(id);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return NoContent();
            }

            return HandleResult(result);
        }
    }
}
