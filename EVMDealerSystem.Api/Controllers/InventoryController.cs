using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : BaseApiController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<InventoryResponse>>>> GetAllInventories()
        {
            var result = await _inventoryService.GetAllInventoriesAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<InventoryResponse>>> GetInventoryById(Guid id)
        {
            var result = await _inventoryService.GetInventoryByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<InventoryResponse>>> CreateInventory([FromBody] InventoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<InventoryResponse>.Invalid("Invalid inventory data.", errors));
            }

            var result = await _inventoryService.CreateInventoryAsync(request);

            if (result.ResultStatus == ResultStatus.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetInventoryById), new { id = result.Data.Id }, result);
            }

            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<InventoryResponse>>> UpdateInventory(Guid id, [FromBody] InventoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return BadRequest(Result<InventoryResponse>.Invalid("Invalid update data.", errors));
            }

            var result = await _inventoryService.UpdateInventoryAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteInventory(Guid id)
        {
            var result = await _inventoryService.DeleteInventoryAsync(id);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return NoContent();
            }

            return HandleResult(result);
        }
    }
}
