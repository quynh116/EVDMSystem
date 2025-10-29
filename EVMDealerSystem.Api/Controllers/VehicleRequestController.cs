using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public async Task<ActionResult<Result<PagedList<VehicleRequestResponse>>>> GetAllRequests([FromQuery] Guid? userId, [FromQuery] VehicleRequestParams parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<PagedList<VehicleRequestResponse>>.Invalid("Invalid pagination or filter parameters."));
            }
            var result = await _vehicleRequestService.GetAllVehicleRequestsAsync(userId, parameters);

            if (result.IsSuccess && result.Data != null)
            {
                var pagedList = result.Data;

                Response.Headers.Append("Pagination", JsonSerializer.Serialize(pagedList.MetaData));

                return Ok(pagedList.ToList());
            }

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

        
        [HttpPost("{id}/approve-manager")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> ApproveByDealerManager(Guid id, [FromQuery] Guid managerId)
        {
            if (managerId == Guid.Empty)
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Manager ID is required."));

            var result = await _vehicleRequestService.ApproveByDealerManagerAsync(id, managerId);
            return HandleResult(result);
        }

        [HttpPost("{id}/reject-manager")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> RejectByDealerManager(Guid id, [FromQuery] Guid managerId, [FromBody] RejectionRequest rejection)
        {
            if (managerId == Guid.Empty)
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Manager ID is required."));
            if (string.IsNullOrWhiteSpace(rejection?.Reason))
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Rejection reason is required in the request body."));

            var result = await _vehicleRequestService.RejectByDealerManagerAsync(id, managerId, rejection.Reason);
            return HandleResult(result);
        }

        [HttpPost("{id}/approve-evm")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> ApproveByEVM(Guid id, [FromQuery] Guid evmStaffId)
        {
            if (evmStaffId == Guid.Empty)
                return BadRequest(Result<VehicleRequestResponse>.Invalid("EVM Staff ID is required."));

            var result = await _vehicleRequestService.ApproveByEVMAsync(id, evmStaffId);
            return HandleResult(result);
        }

        [HttpPost("{id}/reject-evm")]
        public async Task<ActionResult<Result<VehicleRequestResponse>>> RejectByEVM(Guid id, [FromQuery] Guid evmStaffId, [FromBody] RejectionRequest rejection)
        {
            if (evmStaffId == Guid.Empty)
                return BadRequest(Result<VehicleRequestResponse>.Invalid("EVM Staff ID is required."));
            if (string.IsNullOrWhiteSpace(rejection?.Reason))
                return BadRequest(Result<VehicleRequestResponse>.Invalid("Rejection reason is required in the request body."));

            var result = await _vehicleRequestService.RejectByEVMAsync(id, evmStaffId, rejection.Reason);
            return HandleResult(result);
        }
    }
}
