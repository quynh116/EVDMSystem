using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class VehicleRequestService : IVehicleRequestService
    {
        private readonly IVehicleRequestRepository _requestRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDealerRepository _dealerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IVehicleRequestItemRepository _itemRepository;

        public VehicleRequestService(IVehicleRequestRepository requestRepository,
                                     IVehicleRepository vehicleRepository,
                                     IDealerRepository dealerRepository,
                                     IUserRepository userRepository,
                                     IInventoryRepository inventoryRepository,
                                     IVehicleRequestItemRepository itemRepository)
        {
            _requestRepository = requestRepository;
            _vehicleRepository = vehicleRepository;
            _dealerRepository = dealerRepository;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _itemRepository = itemRepository;
        }

        private VehicleRequestItemResponse MapToVehicleRequestItemResponse(VehicleRequestItem item)
        {
            return new VehicleRequestItemResponse
            {
                Id = item.Id,
                VehicleId = item.VehicleId,
                Quantity = item.Quantity,
                VehicleModelName = item.Vehicle?.ModelName ?? "N/A"
            };
        }

        private VehicleRequestResponse MapToVehicleRequestResponse(VehicleRequest request, IEnumerable<VehicleRequestItem> items)
        {
            if (request == null) return null;

            return new VehicleRequestResponse
            {
                Id = request.Id,
                CreatedBy = request.CreatedBy,
                CreatedByName = request.CreatedByNavigation?.FullName ?? "N/A",
                DealerId = request.DealerId,
                DealerName = request.Dealer?.Name ?? "N/A",
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                ApprovedBy = request.ApprovedBy,
                ApprovedByName = request.ApprovedByNavigation?.FullName,
                ApprovedAt = request.ApprovedAt,
                Note = request.Note,
                CanceledAt = request.CanceledAt,
                CanceledBy = request.CanceledBy,
                CancellationReason = request.CancellationReason,
                ExpectedDeliveryDate = request.ExpectedDeliveryDate,
                AllocationConfirmationDate = request.AllocationConfirmationDate,
                Items = items.Select(MapToVehicleRequestItemResponse).ToList()
            };
        }

        public async Task<Result<VehicleRequestResponse>> CreateVehicleRequestAsync(VehicleRequestCreateRequest request)
        {
            try
            {

                var dealer = await _dealerRepository.GetDealerByIdAsync(request.DealerId);
                if (dealer == null) return Result<VehicleRequestResponse>.NotFound($"Dealer ID {request.DealerId} not found.");

                var user = await _userRepository.GetUserByIdAsync(request.CreatedBy);
                if (user == null) return Result<VehicleRequestResponse>.NotFound($"Creating User ID {request.CreatedBy} not found.");

                
                

                
                var newRequest = new VehicleRequest
                {
                    Id = Guid.NewGuid(), 
                    CreatedBy = request.CreatedBy,
                    DealerId = request.DealerId,
                    Note = request.Note,
                    Status = "Pending Manager Approval", 
                    CreatedAt = DateTime.UtcNow
                };

                var addedRequest = await _requestRepository.AddVehicleRequestAsync(newRequest);

                var addedItems = new List<VehicleRequestItem>();
                foreach (var itemDto in request.Items)
                {
                    var vehicle = await _vehicleRepository.GetVehicleByIdAsync(itemDto.VehicleId);
                    if (vehicle == null) continue; 

                    var newItem = new VehicleRequestItem
                    {
                        Id = Guid.NewGuid(),
                        VehicleRequestId = addedRequest.Id,
                        VehicleId = itemDto.VehicleId,
                        Quantity = itemDto.Quantity,
                        CreatedAt = DateTime.UtcNow
                    };



                    // Thêm item vào Repository
                    var addedItem = await _itemRepository.AddItemAsync(newItem); // Cần inject IVehicleRequestItemRepository
                    addedItems.Add(addedItem);
                }


                var requestWithRelations = await _requestRepository.GetVehicleRequestByIdAsync(addedRequest.Id);
                var finalItems = await _itemRepository.GetItemsByRequestIdAsync(addedRequest.Id);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(requestWithRelations, finalItems), "Vehicle request created and stock reserved.");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error creating vehicle request: {ex.Message}");
            }
        }

        public async Task<Result<PagedList<VehicleRequestResponse>>> GetAllVehicleRequestsAsync(Guid? userId, VehicleRequestParams pagingParams)
        {
            try
            {
                IQueryable<VehicleRequest> query = await _requestRepository.GetAllVehicleRequestsAsync();
                if (userId.HasValue && userId != Guid.Empty)
                {
                    var user = await _userRepository.GetUserByIdAsync(userId.Value);

                    if (user != null)
                    {
                        
                        if (user.RoleId == 1)
                        {
                            query = query.Where(r => r.CreatedBy == userId.Value);
                        }

                        else if (user.RoleId == 2 && user.DealerId.HasValue)
                        {
                            query = query.Where(r => r.DealerId == user.DealerId.Value);
                        }

                        
                        else 
                        {
                            query = query.Where(r => r.Status != "Pending Manager Approval");
                        }
                    }
                }

                if (pagingParams.CreatedBy.HasValue && pagingParams.CreatedBy.Value != Guid.Empty)
                {
                    query = query.Where(r => r.CreatedBy == pagingParams.CreatedBy.Value);
                }

                if (!string.IsNullOrWhiteSpace(pagingParams.Status))
                {
                    query = query.Where(r => r.Status != null && r.Status.Equals(pagingParams.Status));
                }
                query = query.OrderByDescending(r => r.CreatedAt);

                var pagedRequests = await PagedList<VehicleRequest>.CreateAsync(
                query,
                pagingParams.PageNumber,
                pagingParams.PageSize);

                var responseItems = pagedRequests.Select(r => MapToVehicleRequestResponse(r, r.VehicleRequestItems)).ToList();

                var pagedResponse = new PagedList<VehicleRequestResponse>(
                responseItems,
                pagedRequests.TotalCount,
                pagedRequests.CurrentPage,
                pagedRequests.PageSize);
                return Result<PagedList<VehicleRequestResponse>>.Success(pagedResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedList<VehicleRequestResponse>>.InternalServerError($"Error retrieving vehicle requests: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> GetVehicleRequestByIdAsync(Guid id)
        {
            try
            {
                var request = await _requestRepository.GetVehicleRequestByIdAsync(id);
                if (request == null)
                {
                    return Result<VehicleRequestResponse>.NotFound($"Vehicle request with ID {id} not found.");
                }
                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(request, request.VehicleRequestItems));
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error retrieving vehicle request: {ex.Message}");
            }
        }

        

       
        public async Task<Result<bool>> DeleteVehicleRequestAsync(Guid id)
        {
            try
            {
                var request = await _requestRepository.GetVehicleRequestByIdAsync(id);
                if (request == null)
                {
                    return Result<bool>.NotFound($"Vehicle request with ID {id} not found.");
                }

                if (request.Status != "Pending")
                {
                    return Result<bool>.Conflict($"Cannot delete request with status: {request.Status}.");
                }

                await _requestRepository.DeleteVehicleRequestAsync(id);
                return Result<bool>.Success(true, "Vehicle request deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting vehicle request: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> ApproveByDealerManagerAsync(Guid requestId, Guid managerId)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);
                if (vehicleRequest == null)
                    return Result<VehicleRequestResponse>.NotFound($"Request ID {requestId} not found.");

                if (vehicleRequest.Status != "Pending Manager Approval")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Cannot approve request. Current status is {vehicleRequest.Status}. Must be 'Pending Manager Approval'.");
                }

                var manager = await _userRepository.GetUserByIdAsync(managerId);
                if (manager == null)
                    return Result<VehicleRequestResponse>.NotFound($"Manager ID {managerId} not found.");

                vehicleRequest.Status = "Pending EVM Allocation"; 
                vehicleRequest.ApprovedBy = managerId;
                vehicleRequest.ApprovedAt = DateTime.UtcNow;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);

                return Result<VehicleRequestResponse>.Success(
                    MapToVehicleRequestResponse(updatedRequest, updatedRequest.VehicleRequestItems),
                    "Vehicle request approved by Manager and sent for EVM allocation."
                );
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error approving vehicle request by Manager: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> RejectByDealerManagerAsync(Guid requestId, Guid managerId, string reason)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);
                if (vehicleRequest == null)
                    return Result<VehicleRequestResponse>.NotFound($"Request ID {requestId} not found.");

                if (vehicleRequest.Status != "Pending Manager Approval")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Cannot reject request. Current status is {vehicleRequest.Status}. Must be 'Pending Manager Approval'.");
                }

                var manager = await _userRepository.GetUserByIdAsync(managerId);
                if (manager == null)
                    return Result<VehicleRequestResponse>.NotFound($"Manager ID {managerId} not found.");

                vehicleRequest.Status = "Canceled by Manager";
                vehicleRequest.CancellationReason = reason;
                vehicleRequest.CanceledBy = managerId;
                vehicleRequest.CanceledAt = DateTime.UtcNow;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);

                return Result<VehicleRequestResponse>.Success(
                    MapToVehicleRequestResponse(updatedRequest, updatedRequest.VehicleRequestItems),
                    $"Vehicle request rejected by Manager: {reason}"
                );
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error rejecting vehicle request by Manager: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> ApproveByEVMAsync(Guid requestId, Guid evmStaffId, EVMApproveRequest request)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);
                if (vehicleRequest == null) return Result<VehicleRequestResponse>.NotFound($"Request ID {requestId} not found.");

                if (vehicleRequest.Status != "Pending EVM Allocation")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Cannot approve request. Current status is {vehicleRequest.Status}. Must be 'Pending EVM Allocation'.");
                }

                var evmStaff = await _userRepository.GetUserByIdAsync(evmStaffId);
                if (evmStaff == null)
                    return Result<VehicleRequestResponse>.NotFound($"EVM Staff ID {evmStaffId} not found or unauthorized.");

                // --- BƯỚC 1: KIỂM TRA VÀ PHÂN BỔ TỒN KHO CHO NHIỀU ITEM ---

                var allUnitsToAllocate = new List<Inventory>();
                var itemsToCheck = vehicleRequest.VehicleRequestItems.ToList();

                // Kiểm tra và thu thập tất cả các đơn vị Inventory cần thiết
                foreach (var item in itemsToCheck)
                {
                    var availableStock = await _inventoryRepository.FindAvailableStockForAllocationAsync(
                        item.VehicleId,
                        item.Quantity);

                    if (availableStock.Count() < item.Quantity)
                    {
                        // Xử lý lỗi thiếu kho (như code trước)
                        return Result<VehicleRequestResponse>.Conflict(
                            $"Insufficient stock for Vehicle **{item.Vehicle.ModelName}**." +
                            "Please reject the request and provide a reason."
                        );
                    }

                    allUnitsToAllocate.AddRange(availableStock.Take(item.Quantity));
                }

                // --- BƯỚC 2: CẬP NHẬT TỒN KHO & NGÀY SHIPPING ---

                Guid currentRequestId = vehicleRequest.Id;
                Guid dealerId = vehicleRequest.DealerId;
                DateTime shippingDate = DateTime.UtcNow; // Ngày xe rời kho Hãng

                foreach (var inventory in allUnitsToAllocate)
                {
                    inventory.DealerId = dealerId;
                    inventory.Status = "Shipped to Dealer";
                    inventory.VehicleRequestId = currentRequestId;
                    inventory.UpdatedAt = DateTime.UtcNow;

                    // THÊM: Cập nhật ShippingDate cho từng Inventory
                    inventory.ShippingDate = shippingDate;
                }

                await _inventoryRepository.UpdateRangeInventoryAsync(allUnitsToAllocate);

                // --- BƯỚC 3: CẬP NHẬT REQUEST CHÍNH & NGÀY DỰ KIẾN GIAO HÀNG ---

                // Dùng thuộc tính riêng biệt cho EVM
                vehicleRequest.Status = "Shipped";
                vehicleRequest.ApprovedBy = evmStaffId;
                vehicleRequest.ApprovedAt = DateTime.UtcNow;

                // THÊM: Cập nhật ExpectedDeliveryDate từ request DTO
                vehicleRequest.ExpectedDeliveryDate = request.ExpectedDeliveryDate;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);

                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);

                return Result<VehicleRequestResponse>.Success(
                    MapToVehicleRequestResponse(updatedRequest, updatedRequest.VehicleRequestItems),
                    $"Inventory allocated and shipped successfully. Expected Delivery: {request.ExpectedDeliveryDate:d}."
                );
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error approving and allocating inventory: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> RejectByEVMAsync(Guid requestId, Guid evmStaffId, string reason)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);
                if (vehicleRequest == null) return Result<VehicleRequestResponse>.NotFound($"Request ID {requestId} not found.");

                if (vehicleRequest.Status != "Pending EVM Allocation")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Cannot reject request. Current status is {vehicleRequest.Status}.");
                }

                var evmStaff = await _userRepository.GetUserByIdAsync(evmStaffId);
                if (evmStaff == null) return Result<VehicleRequestResponse>.NotFound($"EVM Staff ID {evmStaffId} not found.");

                vehicleRequest.Status = "Canceled by EVM";
                vehicleRequest.CancellationReason = reason; 
                vehicleRequest.CanceledBy = evmStaffId;
                vehicleRequest.CanceledAt = DateTime.UtcNow;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(updatedRequest, updatedRequest.VehicleRequestItems), $"Vehicle request canceled by EVM: {reason}");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error rejecting vehicle request: {ex.Message}");
            }
        }
    }
}
