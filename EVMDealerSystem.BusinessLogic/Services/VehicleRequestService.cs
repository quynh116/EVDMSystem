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

        public VehicleRequestService(IVehicleRequestRepository requestRepository,
                                     IVehicleRepository vehicleRepository,
                                     IDealerRepository dealerRepository,
                                     IUserRepository userRepository,
                                     IInventoryRepository inventoryRepository)
        {
            _requestRepository = requestRepository;
            _vehicleRepository = vehicleRepository;
            _dealerRepository = dealerRepository;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
        }

        private VehicleRequestResponse MapToVehicleRequestResponse(VehicleRequest request)
        {
            if (request == null) return null;

            return new VehicleRequestResponse
            {
                Id = request.Id,
                CreatedBy = request.CreatedBy,
                CreatedByName = request.CreatedByNavigation?.FullName ?? "N/A",
                VehicleId = request.VehicleId,
                VehicleModelName = request.Vehicle?.ModelName ?? "N/A",
                DealerId = request.DealerId,
                DealerName = request.Dealer?.Name ?? "N/A",
                Quantity = request.Quantity,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                ApprovedBy = request.ApprovedBy,
                ApprovedByName = request.ApprovedByNavigation?.FullName,
                ApprovedAt = request.ApprovedAt,
                Note = request.Note,
                CanceledAt = request.CanceledAt,
                CanceledBy = request.CanceledBy,
                CancellationReason = request.CancellationReason
            };
        }

        public async Task<Result<VehicleRequestResponse>> CreateVehicleRequestAsync(VehicleRequestCreateRequest request)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(request.VehicleId);
                if (vehicle == null) return Result<VehicleRequestResponse>.NotFound($"Vehicle ID {request.VehicleId} not found.");

                var dealer = await _dealerRepository.GetDealerByIdAsync(request.DealerId);
                if (dealer == null) return Result<VehicleRequestResponse>.NotFound($"Dealer ID {request.DealerId} not found.");

                var user = await _userRepository.GetUserByIdAsync(request.CreatedBy);
                if (user == null) return Result<VehicleRequestResponse>.NotFound($"Creating User ID {request.CreatedBy} not found.");

                
                

                
                var newRequest = new VehicleRequest
                {
                    Id = Guid.NewGuid(), 
                    CreatedBy = request.CreatedBy,
                    VehicleId = request.VehicleId,
                    DealerId = request.DealerId,
                    Quantity = request.Quantity,
                    Note = request.Note,
                    Status = "Pending Manager Approval", 
                    CreatedAt = DateTime.UtcNow
                };

                var addedRequest = await _requestRepository.AddVehicleRequestAsync(newRequest);

               
                

                var requestWithRelations = await _requestRepository.GetVehicleRequestByIdAsync(addedRequest.Id);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(requestWithRelations), "Vehicle request created and stock reserved.");
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

                var responseItems = pagedRequests.Select(r => MapToVehicleRequestResponse(r)).ToList();

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
                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(request));
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error retrieving vehicle request: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> UpdateVehicleRequestAsync(Guid id, VehicleRequestUpdateRequest request)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(id);
                if (vehicleRequest == null)
                {
                    return Result<VehicleRequestResponse>.NotFound($"Vehicle request with ID {id} not found.");
                }

                if (vehicleRequest.Status != "Pending")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Cannot update request with status: {vehicleRequest.Status}.");
                }

                if (request.Quantity.HasValue) vehicleRequest.Quantity = request.Quantity.Value;
                if (request.Note != null) vehicleRequest.Note = request.Note;


                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(vehicleRequest.Id);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(updatedRequest), "Vehicle request updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error updating vehicle request: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> ApproveVehicleRequestAsync(Guid requestId, Guid evmStaffId)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);
                if (vehicleRequest == null) return Result<VehicleRequestResponse>.NotFound($"Request ID {requestId} not found.");

                
                if (vehicleRequest.Status != "Processing")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Request status is {vehicleRequest.Status}. Must be 'Processing' to allocate.");
                }

                
                var evmStaff = await _userRepository.GetUserByIdAsync(evmStaffId);
                if (evmStaff == null) return Result<VehicleRequestResponse>.NotFound($"EVM Staff ID {evmStaffId} not found.");

                
                var reservedInventories = await _inventoryRepository.GetReservedInventoryByRequestIdAsync(requestId);

                if (reservedInventories.Count() != vehicleRequest.Quantity)
                {
                    
                    return Result<VehicleRequestResponse>.InternalServerError("Stock integrity error: Reserved quantity does not match request quantity. Allocation failed.");
                }

                
                foreach (var inventory in reservedInventories)
                {
                    inventory.DealerId = vehicleRequest.DealerId;     
                    inventory.Status = "Allocated to Dealer";         
                    inventory.VehicleRequestId = null;                
                    inventory.UpdatedAt = DateTime.UtcNow;
                }

                await _inventoryRepository.UpdateRangeInventoryAsync(reservedInventories);

                
                vehicleRequest.Status = "completed";
                vehicleRequest.ApprovedBy = evmStaffId;
                vehicleRequest.ApprovedAt = DateTime.UtcNow;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(updatedRequest), $"Inventory allocated successfully. {vehicleRequest.Quantity} units moved to Dealer {vehicleRequest.Dealer.Name}.");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error allocating inventory: {ex.Message}");
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
                    MapToVehicleRequestResponse(updatedRequest),
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
                    MapToVehicleRequestResponse(updatedRequest),
                    $"Vehicle request rejected by Manager: {reason}"
                );
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error rejecting vehicle request by Manager: {ex.Message}");
            }
        }

        public async Task<Result<VehicleRequestResponse>> ApproveByEVMAsync(Guid requestId, Guid evmStaffId)
        {
            try
            {
                var vehicleRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);
                if (vehicleRequest == null) return Result<VehicleRequestResponse>.NotFound($"Request ID {requestId} not found.");

                if (vehicleRequest.Status != "Pending EVM Allocation")
                {
                    return Result<VehicleRequestResponse>.Conflict($"Cannot approve request. Current status is {vehicleRequest.Status}.");
                }

                var evmStaff = await _userRepository.GetUserByIdAsync(evmStaffId);
                if (evmStaff == null) return Result<VehicleRequestResponse>.NotFound($"EVM Staff ID {evmStaffId} not found.");

                var availableStock = await _inventoryRepository.FindAvailableStockForRequestAsync(
                    vehicleRequest.VehicleId,
                    vehicleRequest.Quantity);

                if (availableStock.Count() < vehicleRequest.Quantity)
                {
                    int availableCount = availableStock.Count();
                    int requestedQuantity = vehicleRequest.Quantity;
                    return Result<VehicleRequestResponse>.Conflict(
                        $"Insufficient stock for allocation. Currently, **only {availableCount} vehicles are available to ship,  while the dealership requested **{requestedQuantity} vehicles**." +
                        "Please reject the request and provide a reason."
                    );
                }

                Guid currentRequestId = vehicleRequest.Id;
                foreach (var inventory in availableStock)
                {
                    inventory.DealerId = vehicleRequest.DealerId;     
                    inventory.Status = "Allocated to Dealer";         
                    inventory.VehicleRequestId = currentRequestId;                
                    inventory.UpdatedAt = DateTime.UtcNow;
                }

                await _inventoryRepository.UpdateRangeInventoryAsync(availableStock);

                vehicleRequest.Status = "completed";
                vehicleRequest.ApprovedBy = evmStaffId;
                vehicleRequest.ApprovedAt = DateTime.UtcNow;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(requestId);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(updatedRequest), $"Inventory allocated successfully. {vehicleRequest.Quantity} units moved.");
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

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(updatedRequest), $"Vehicle request canceled by EVM: {reason}");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error rejecting vehicle request: {ex.Message}");
            }
        }
    }
}
