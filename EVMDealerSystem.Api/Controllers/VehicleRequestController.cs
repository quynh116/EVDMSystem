using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleRequestController : BaseApiController
    {
        private readonly IVehicleRequestService _vehicleRequestService;

        public VehicleRequestController(IVehicleRequestService vehicleRequestService)
        {
            _vehicleRequestService = vehicleRequestService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<VehicleRequestResponse>>>> GetAllRequests()
        {
            var result = await _vehicleRequestService.GetAllVehicleRequestsAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> GetRequestById(Guid id)
        {
            var result = await _vehicleRequestService.GetVehicleRequestByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> CreateRequest([FromBody] VehicleRequestCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Invalid request data.", errors));
            }

            var result = await _vehicleRequestService.CreateVehicleRequestAsync(request);

            if (result.ResultStatus == ResultStatus.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetRequestById), new { id = result.Data.Id }, result);
            }

            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> UpdateRequest(Guid id, [FromBody] VehicleRequestUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Invalid update data.", errors));
            }

            var result = await _vehicleRequestService.UpdateVehicleRequestAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteRequest(Guid id)
        {
            var result = await _vehicleRequestService.DeleteVehicleRequestAsync(id);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return NoContent();
            }

            return HandleResult(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> ApproveRequest(Guid id, [FromQuery] Guid approverId)
        {
            if (approverId == Guid.Empty)
            {
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Approver ID is required."));
            }

            var result = await _vehicleRequestService.ApproveVehicleRequestAsync(id, approverId);
            return HandleResult(result);
        }
    }
}
