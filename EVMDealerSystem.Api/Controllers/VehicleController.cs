using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Request.Vehicle;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Models.Responses.VehicleResponse;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : BaseApiController
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<VehicleResponse>>>> GetAllVehicles()
        {
            var result = await _vehicleService.GetAllVehiclesAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<VehicleResponse>>> GetVehicleById(Guid id)
        {
            var result = await _vehicleService.GetVehicleByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<VehicleResponse>>> CreateVehicle([FromBody] VehicleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<VehicleResponse>.Invalid("Invalid vehicle data.", errors));
            }

            var result = await _vehicleService.CreateVehicleAsync(request);

            if (result.ResultStatus == ResultStatus.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetVehicleById), new { id = result.Data.Id }, result);
            }

            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<VehicleResponse>>> UpdateVehicle(Guid id, [FromBody] VehicleUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<VehicleResponse>.Invalid("Invalid update data.", errors));
            }

            var result = await _vehicleService.UpdateVehicleAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteVehicle(Guid id)
        {
            var result = await _vehicleService.DeleteVehicleAsync(id);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return NoContent();
            }

            return HandleResult(result);
        }

        [HttpPost("{id}/inventory")] 
        public async Task<ActionResult<Result<IEnumerable<InventoryResponse>>>> AddInventoryBatch([FromBody] InventoryBatchCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<IEnumerable<InventoryResponse>>.Invalid("Invalid inventory data.", errors));
            }

            

            var result = await _vehicleService.AddInventoryBatchAsync(request);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return HandleResult(result);
        }
    }
}
