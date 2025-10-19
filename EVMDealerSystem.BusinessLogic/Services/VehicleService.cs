using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Request.Vehicle;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Models.Responses.VehicleResponse;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IEvmRepository _evmRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public VehicleService(IVehicleRepository vehicleRepository, IEvmRepository evmRepository, IInventoryRepository inventoryRepository)
        {
            _vehicleRepository = vehicleRepository;
            _evmRepository = evmRepository;
            _inventoryRepository = inventoryRepository;
        }
        private VehicleResponse MapToVehicleResponse(Vehicle vehicle)
        {
            if (vehicle == null) return null;

            return new VehicleResponse
            {
                Id = vehicle.Id,
                ModelName = vehicle.ModelName,
                Version = vehicle.Version,
                Category = vehicle.Category,
                Color = vehicle.Color,
                ImageUrl = vehicle.ImageUrl,
                Description = vehicle.Description,
                BatteryCapacity = vehicle.BatteryCapacity,
                RangePerCharge = vehicle.RangePerCharge,
                BasePrice = vehicle.BasePrice,
                Status = vehicle.Status,
                LaunchDate = vehicle.LaunchDate,
                EvmId = vehicle.EvmId,
                EvmName = vehicle.Evm?.Name, 
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt
            };
        }

        public async Task<Result<VehicleResponse>> CreateVehicleAsync(VehicleCreateRequest request)
        {
            try
            {
                var evms = await _evmRepository.GetAllEvmsAsync();
                var evm = evms.FirstOrDefault();

                if (evm == null)
                {
                    return Result<VehicleResponse>.Conflict("Manufacturer (EVM) not found. Cannot create vehicle.");
                }

                var newVehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    ModelName = request.ModelName,
                    Version = request.Version,
                    Category = request.Category,
                    Color = request.Color,
                    ImageUrl = request.ImageUrl,
                    Description = request.Description,
                    BatteryCapacity = request.BatteryCapacity,
                    RangePerCharge = request.RangePerCharge,
                    BasePrice = request.BasePrice,
                    Status =  "Available", 
                    LaunchDate = request.LaunchDate,
                    EvmId = evm.Id, 
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                var addedVehicle = await _vehicleRepository.AddVehicleAsync(newVehicle);

                var vehicleWithEvm = await _vehicleRepository.GetVehicleByIdAsync(addedVehicle.Id);

                return Result<VehicleResponse>.Success(MapToVehicleResponse(vehicleWithEvm), "Vehicle created successfully.");
            }
            catch (Exception ex)
            {
                return Result<VehicleResponse>.InternalServerError($"Error creating vehicle: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleResponse>>> GetAllVehiclesAsync()
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllVehiclesAsync();
                var responses = vehicles.Select(v => MapToVehicleResponse(v)).ToList();
                return Result<IEnumerable<VehicleResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleResponse>>.InternalServerError($"Error retrieving vehicles: {ex.Message}");
            }
        }

        public async Task<Result<VehicleResponse>> GetVehicleByIdAsync(Guid id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<VehicleResponse>.NotFound($"Vehicle with ID {id} not found.");
                }
                return Result<VehicleResponse>.Success(MapToVehicleResponse(vehicle));
            }
            catch (Exception ex)
            {
                return Result<VehicleResponse>.InternalServerError($"Error retrieving vehicle: {ex.Message}");
            }
        }

        public async Task<Result<VehicleResponse>> UpdateVehicleAsync(Guid id, VehicleUpdateRequest request)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<VehicleResponse>.NotFound($"Vehicle with ID {id} not found.");
                }

                if (request.ModelName != null) vehicle.ModelName = request.ModelName;
                if (request.Version != null) vehicle.Version = request.Version;
                if (request.Category != null) vehicle.Category = request.Category;
                if (request.Color != null) vehicle.Color = request.Color;
                if (request.ImageUrl != null) vehicle.ImageUrl = request.ImageUrl;
                if (request.Description != null) vehicle.Description = request.Description;

                if (request.BatteryCapacity.HasValue) vehicle.BatteryCapacity = request.BatteryCapacity.Value;
                if (request.RangePerCharge.HasValue) vehicle.RangePerCharge = request.RangePerCharge.Value;
                if (request.BasePrice.HasValue) vehicle.BasePrice = request.BasePrice.Value;
                if (request.Status != null) vehicle.Status = request.Status;
                if (request.LaunchDate.HasValue) vehicle.LaunchDate = request.LaunchDate.Value;

                vehicle.UpdatedAt = DateTime.UtcNow;

                await _vehicleRepository.UpdateVehicleAsync(vehicle);

                var updatedVehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicle.Id);

                return Result<VehicleResponse>.Success(MapToVehicleResponse(updatedVehicle), "Vehicle updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<VehicleResponse>.InternalServerError($"Error updating vehicle: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteVehicleAsync(Guid id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<bool>.NotFound($"Vehicle with ID {id} not found.");
                }


                await _vehicleRepository.DeleteVehicleAsync(id);
                return Result<bool>.Success(true, "Vehicle deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting vehicle: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InventoryResponse>>> AddInventoryBatchAsync(InventoryBatchCreateRequest request)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(request.VehicleId);
                if (vehicle == null)
                {
                    return Result<IEnumerable<InventoryResponse>>.NotFound($"Vehicle with ID {request.VehicleId} not found.");
                }

                var inventories = new List<Inventory>();

                for (int i = 0; i < request.Quantity; i++)
                {
                    inventories.Add(new Inventory
                    {
                        Id = Guid.NewGuid(),
                        VehicleId = request.VehicleId,
                        DealerId = null, 
                        VinNumber = $"VIN-{vehicle.ModelName.Replace(" ", "").ToUpper()}-{DateTime.Now.Ticks}-{i}", 
                        Status = "At Manufacturer", 
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null
                    });
                }

                
                await _inventoryRepository.AddRangeInventoryAsync(inventories);

                

                var responseMessage = $"{request.Quantity} units of {vehicle.ModelName} added to manufacturer inventory.";
                return Result<IEnumerable<InventoryResponse>>.Success(new List<InventoryResponse>(), responseMessage);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InventoryResponse>>.InternalServerError($"Error adding inventory batch: {ex.Message}");
            }
        }
    }
}
