using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest;
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
    public class VehicleRequestService : IVehicleRequestService
    {
        private readonly IVehicleRequestRepository _requestRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDealerRepository _dealerRepository;
        private readonly IUserRepository _userRepository; // Giả định Repository cho User

        public VehicleRequestService(IVehicleRequestRepository requestRepository,
                                     IVehicleRepository vehicleRepository,
                                     IDealerRepository dealerRepository,
                                     IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _vehicleRepository = vehicleRepository;
            _dealerRepository = dealerRepository;
            _userRepository = userRepository;
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
                Note = request.Note
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
                    Status = "Pending", 
                    CreatedAt = DateTime.UtcNow
                };

                var addedRequest = await _requestRepository.AddVehicleRequestAsync(newRequest);

                var requestWithRelations = await _requestRepository.GetVehicleRequestByIdAsync(addedRequest.Id);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(requestWithRelations), "Vehicle request created and pending approval.");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error creating vehicle request: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleRequestResponse>>> GetAllVehicleRequestsAsync()
        {
            try
            {
                var requests = await _requestRepository.GetAllVehicleRequestsAsync();
                var responses = requests.Select(r => MapToVehicleRequestResponse(r)).ToList();
                return Result<IEnumerable<VehicleRequestResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleRequestResponse>>.InternalServerError($"Error retrieving vehicle requests: {ex.Message}");
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

        public async Task<Result<VehicleRequestResponse>> ApproveVehicleRequestAsync(Guid id, Guid approverId)
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
                    return Result<VehicleRequestResponse>.Conflict($"Request is already {vehicleRequest.Status}.");
                }

                var approver = await _userRepository.GetUserByIdAsync(approverId);
                if (approver == null) return Result<VehicleRequestResponse>.NotFound($"Approver User ID {approverId} not found.");


                vehicleRequest.Status = "Approved";
                vehicleRequest.ApprovedBy = approverId;
                vehicleRequest.ApprovedAt = DateTime.UtcNow;

                await _requestRepository.UpdateVehicleRequestAsync(vehicleRequest);
                var updatedRequest = await _requestRepository.GetVehicleRequestByIdAsync(vehicleRequest.Id);

                return Result<VehicleRequestResponse>.Success(MapToVehicleRequestResponse(updatedRequest), "Vehicle request approved successfully.");
            }
            catch (Exception ex)
            {
                return Result<VehicleRequestResponse>.InternalServerError($"Error approving vehicle request: {ex.Message}");
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
    }
}
