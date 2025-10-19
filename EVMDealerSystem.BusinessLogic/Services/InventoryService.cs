using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDealerRepository _dealerRepository;

        public InventoryService(IInventoryRepository inventoryRepository, IVehicleRepository vehicleRepository, IDealerRepository dealerRepository)
        {
            _inventoryRepository = inventoryRepository;
            _vehicleRepository = vehicleRepository;
            _dealerRepository = dealerRepository;
        }

        private InventoryResponse MapToInventoryResponse(Inventory inventory)
        {
            if (inventory == null) return null;

            return new InventoryResponse
            {
                Id = inventory.Id,
                VehicleId = inventory.VehicleId,
                VehicleModelName = inventory.Vehicle?.ModelName ?? "N/A", 
                DealerId = inventory.DealerId,
                DealerName = inventory.Dealer?.Name ?? "N/A", 
                VehicleRequestId = inventory.VehicleRequestId,
                VinNumber = inventory.VinNumber,
                Status = inventory.Status,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt
            };
        }

        public async Task<Result<InventoryResponse>> CreateInventoryAsync(InventoryCreateRequest request)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(request.VehicleId);
                if (vehicle == null)
                {
                    return Result<InventoryResponse>.NotFound($"Vehicle with ID {request.VehicleId} not found.");
                }

                

                // 2. Kiểm tra nghiệp vụ (ví dụ: VIN có bị trùng không)

                // 3. Tạo đối tượng Inventory
                var newInventory = new Inventory
                {
                    Id = Guid.NewGuid(),
                    VehicleId = request.VehicleId,
                    DealerId = request.DealerId,
                    VinNumber = request.VinNumber.Trim().ToUpper(),
                    Status = request.Status ?? "In Stock", 
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                var addedInventory = await _inventoryRepository.AddInventoryAsync(newInventory);

                var inventoryWithRelations = await _inventoryRepository.GetInventoryByIdAsync(addedInventory.Id);

                return Result<InventoryResponse>.Success(MapToInventoryResponse(inventoryWithRelations), "Inventory record created successfully.");
            }
            catch (Exception ex)
            {
                return Result<InventoryResponse>.InternalServerError($"Error creating inventory record: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InventoryResponse>>> GetAllInventoriesAsync()
        {
            try
            {
                var inventories = await _inventoryRepository.GetAllInventoriesAsync();
                var responses = inventories.Select(i => MapToInventoryResponse(i)).ToList();
                return Result<IEnumerable<InventoryResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InventoryResponse>>.InternalServerError($"Error retrieving inventories: {ex.Message}");
            }
        }

        public async Task<Result<InventoryResponse>> GetInventoryByIdAsync(Guid id)
        {
            try
            {
                var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
                if (inventory == null)
                {
                    return Result<InventoryResponse>.NotFound($"Inventory record with ID {id} not found.");
                }
                return Result<InventoryResponse>.Success(MapToInventoryResponse(inventory));
            }
            catch (Exception ex)
            {
                return Result<InventoryResponse>.InternalServerError($"Error retrieving inventory record: {ex.Message}");
            }
        }

        public async Task<Result<InventoryResponse>> UpdateInventoryAsync(Guid id, InventoryUpdateRequest request)
        {
            try
            {
                var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
                if (inventory == null)
                {
                    return Result<InventoryResponse>.NotFound($"Inventory record with ID {id} not found.");
                }

                if (request.VinNumber != null) inventory.VinNumber = request.VinNumber.Trim().ToUpper();
                if (request.Status != null) inventory.Status = request.Status;

                inventory.UpdatedAt = DateTime.UtcNow;

                await _inventoryRepository.UpdateInventoryAsync(inventory);

                var updatedInventory = await _inventoryRepository.GetInventoryByIdAsync(inventory.Id);

                return Result<InventoryResponse>.Success(MapToInventoryResponse(updatedInventory), "Inventory record updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<InventoryResponse>.InternalServerError($"Error updating inventory record: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteInventoryAsync(Guid id)
        {
            try
            {
                var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
                if (inventory == null)
                {
                    return Result<bool>.NotFound($"Inventory record with ID {id} not found.");
                }


                await _inventoryRepository.DeleteInventoryAsync(id);
                return Result<bool>.Success(true, "Inventory record deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting inventory record: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InventoryResponse>>> GetInventoriesAtManufacturerAsync()
        {
            try
            {
                var inventories = await _inventoryRepository.GetInventoriesAtManufacturerAsync();
                var responses = inventories.Select(i => MapToInventoryResponse(i)).ToList();
                return Result<IEnumerable<InventoryResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InventoryResponse>>.InternalServerError($"Error retrieving manufacturer inventory: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetAvailableStockQuantityForVehicleAsync(Guid vehicleId)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    return Result<int>.NotFound($"Vehicle with ID {vehicleId} not found.");
                }

                var quantity = await _inventoryRepository.CountAvailableStockByVehicleIdAsync(vehicleId);

                return Result<int>.Success(quantity);
            }
            catch (Exception ex)
            {
                return Result<int>.InternalServerError($"Error retrieving available stock quantity: {ex.Message}");
            }
        }
    }
}
