using Azure;
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
        private readonly IUserRepository _userRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IDealerVehiclePriceRepository _dealerPriceRepository;
        private readonly IPhotoService _photoService;

        public VehicleService(IVehicleRepository vehicleRepository, IEvmRepository evmRepository, IInventoryRepository inventoryRepository, IUserRepository userRepository, IPromotionRepository promotionRepository, IDealerVehiclePriceRepository dealerPriceRepository, IPhotoService photoService)
        {
            _vehicleRepository = vehicleRepository;
            _evmRepository = evmRepository;
            _inventoryRepository = inventoryRepository;
            _userRepository = userRepository;
            _promotionRepository = promotionRepository;
            _dealerPriceRepository = dealerPriceRepository;
            _photoService = photoService;

        }
        private VehicleResponse MapToVehicleResponse(Vehicle vehicle, decimal finalPrice, int currentStock, decimal? dealerSellingPrice)
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
                FinalPrice = finalPrice,
                SellingPrice = dealerSellingPrice,
                CurrentStock = currentStock,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt
            };
        }

        private VehicleResponse MapToVehicleResponse(Vehicle vehicle)
        {
            if (vehicle == null) return null;

            decimal defaultFinalPrice = vehicle.BasePrice;
            int defaultCurrentStock = 0;

            return MapToVehicleResponse(vehicle, defaultFinalPrice, defaultCurrentStock, null);
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

                string imageUrl = null;

                if (request.ImageUrl != null)
                {
                    imageUrl = await _photoService.UploadImageAsync(request.ImageUrl);

                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        return Result<VehicleResponse>.InternalServerError("Image upload failed.");
                    }
                }

                var newVehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    ModelName = request.ModelName,
                    Version = request.Version,
                    Category = request.Category,
                    Color = request.Color,
                    ImageUrl = imageUrl,
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

        public async Task<Result<IEnumerable<VehicleResponse>>> GetAllVehiclesAsync(Guid? userId)
        {
            try
            {
                Guid? targetDealerId = null;
                decimal? dealerSellingPrice = null;

                if (userId.HasValue && userId != Guid.Empty)
                {
                    var user = await _userRepository.GetUserByIdAsync(userId.Value);

                    
                    if (user != null && user.DealerId.HasValue)
                    {
                        targetDealerId = user.DealerId.Value;
                    }
                    
                }

                
                var stockMap = await _inventoryRepository.GetStockCountByVehicleAndDealerAsync(targetDealerId);

                
                var vehicles = await _vehicleRepository.GetAllVehiclesAsync();

                var responses = new List<VehicleResponse>();

                
                Dictionary<Guid, decimal> dealerPricesMap = new Dictionary<Guid, decimal>();
                if (targetDealerId.HasValue)
                {
                    
                    var dealerPrices = await _dealerPriceRepository.GetAllPricesByDealerAsync(targetDealerId.Value);
                    dealerPricesMap = dealerPrices.ToDictionary(p => p.VehicleId, p => p.SellingPrice);
                }

                foreach (var v in vehicles)
                {
                    int currentStock = stockMap.GetValueOrDefault(v.Id, 0);

                    decimal finalPrice = await CalculateFinalPriceAsync(v.Id, v.BasePrice,targetDealerId);

                    if (dealerPricesMap.ContainsKey(v.Id))
                    {
                        dealerSellingPrice = dealerPricesMap[v.Id];
                    }
                    else
                    {
                        dealerSellingPrice = null; 
                    }

                    var response = MapToVehicleResponse(v, finalPrice, currentStock, dealerSellingPrice);
                    response.DealerId = targetDealerId; 

                    responses.Add(response);
                }

                return Result<IEnumerable<VehicleResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleResponse>>.InternalServerError($"Error retrieving vehicles: {ex.Message}");
            }
        }

        public async Task<Result<VehicleResponse>> GetVehicleByIdAsync(Guid id, Guid? userId)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<VehicleResponse>.NotFound($"Vehicle with ID {id} not found.");
                }

                Guid? targetDealerId = null;
                if (userId.HasValue && userId != Guid.Empty)
                {
                    var user = await _userRepository.GetUserByIdAsync(userId.Value);
                    if (user != null && user.DealerId.HasValue)
                    {
                        targetDealerId = user.DealerId.Value;
                    }
                }
                int currentStock = 0;
                var stockMap = await _inventoryRepository.GetStockCountByVehicleAndDealerAsync(targetDealerId);
                if (stockMap.ContainsKey(id))
                {
                    currentStock = stockMap[id];
                }

                decimal? dealerSellingPrice = null;
                if (targetDealerId.HasValue)
                {
                    var dealerPrice = await _dealerPriceRepository.GetPriceByDealerAndVehicleAsync(targetDealerId.Value, id);
                    dealerSellingPrice = dealerPrice?.SellingPrice;
                }

                decimal finalPrice = await CalculateFinalPriceAsync(id, vehicle.BasePrice, targetDealerId);

                

                var response = MapToVehicleResponse(vehicle, finalPrice, currentStock, dealerSellingPrice);
                response.DealerId = targetDealerId; 

                return Result<VehicleResponse>.Success(response);
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

        private async Task<decimal> CalculateFinalPriceAsync(Guid vehicleId, decimal basePrice, Guid? dealerId)
        {
            decimal discountBasePrice = basePrice;
            decimal? dealerSellingPrice = null;

            if (dealerId.HasValue)
            {
                var dealerPrice = await _dealerPriceRepository.GetPriceByDealerAndVehicleAsync(dealerId.Value, vehicleId);
                dealerSellingPrice = dealerPrice?.SellingPrice;

                if (dealerSellingPrice.HasValue)
                {
                    discountBasePrice = dealerSellingPrice.Value;
                }

                var promotions = await _promotionRepository.GetActivePromotionsByVehicleIdAsync(vehicleId, dealerId.Value);

                decimal finalPrice = discountBasePrice; 

                foreach (var p in promotions)
                {
                    if (p.DiscountType == "Percent" && p.DiscountPercent.HasValue)
                    {
                        decimal discountAmount = discountBasePrice * (p.DiscountPercent.Value / 100m);
                        finalPrice -= discountAmount;
                    }
                }

                return Math.Max(0, finalPrice);
            }

            return basePrice;
        }

        public async Task<Result<bool>> SetDealerSellingPriceAsync(Guid userId, DealerSellingPriceRequest request)
        {
            try
            {
                // 1. Xác định DealerId từ UserId
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return Result<bool>.NotFound("User not found.");

                if (!user.DealerId.HasValue)
                    return Result<bool>.NotFound("User does not belong to a Dealer.");

                Guid dealerId = user.DealerId.Value;

                // 2. Kiểm tra Vehicle tồn tại
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(request.VehicleId);
                if (vehicle == null)
                    return Result<bool>.NotFound($"Vehicle ID {request.VehicleId} not found.");

                // 3. Tạo/Cập nhật đối tượng DealerVehiclePrice
                var newPriceEntry = new DealerVehiclePrice
                {
                    DealerId = dealerId,
                    VehicleId = request.VehicleId,
                    SellingPrice = request.SellingPrice,
                    // CreatedAt/UpdatedAt sẽ được xử lý trong Repository
                };

                // 4. Gọi Repository để Set/Update
                await _dealerPriceRepository.SetOrUpdatePriceAsync(newPriceEntry);

                return Result<bool>.Success(true, "Dealer selling price set successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error setting dealer price: {ex.Message}");
            }
        }
    }
}
