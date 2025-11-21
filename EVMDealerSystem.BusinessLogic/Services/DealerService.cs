using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class DealerService : IDealerService
    {
        private readonly IDealerRepository _dealerRepository;
        private readonly IEvmRepository _evmRepository;

        public DealerService(IDealerRepository dealerRepository, IEvmRepository evmRepository)
        {
            _dealerRepository = dealerRepository;
            _evmRepository = evmRepository;
        }

        private DealerResponse MapToDealerResponse(Dealer dealer)
        {
            if (dealer == null) return null;

            return new DealerResponse
            {
                Id = dealer.Id,
                Name = dealer.Name,
                Address = dealer.Address,
                Phone = dealer.Phone,
                Email = dealer.Email,
                EvmId = dealer.EvmId,
                EvmName = dealer.Evm?.Name,
                ContractNumber = dealer.ContractNumber,
                SalesTarget = dealer.SalesTarget,
                IsActive = dealer.IsActive,
                CreatedAt = dealer.CreatedAt,
                UpdatedAt = dealer.UpdatedAt
            };
        }

        public async Task<Result<DealerResponse>> CreateDealerAsync(DealerRequest request)
        {
            try
            {
                var evms = await _evmRepository.GetAllEvmsAsync();
                var evm = evms.FirstOrDefault();

                if (evm == null)
                {
                    return Result<DealerResponse>.Conflict("Manufacturer (EVM) not found. Cannot create dealer.");
                }

                var newDealer = new Dealer
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Address = request.Address,
                    Phone = request.Phone,
                    Email = request.Email,
                    ContractNumber = request.ContractNumber,
                    SalesTarget = request.SalesTarget,
                    IsActive = request.IsActive ?? true,
                    EvmId = evm.Id,
                    CreatedAt = TimeHelper.GetVietNamTime(),
                    UpdatedAt = null
                };

                // 3. Thêm vào DB
                var addedDealer = await _dealerRepository.AddDealerAsync(newDealer);

                var dealerWithEvm = await _dealerRepository.GetDealerByIdAsync(addedDealer.Id);

                return Result<DealerResponse>.Success(MapToDealerResponse(dealerWithEvm), "Dealer created successfully.");
            }
            catch (Exception ex)
            {
                return Result<DealerResponse>.InternalServerError($"Error creating dealer: {ex.Message}");
            }
        }
        public async Task<Result<IEnumerable<DealerResponse>>> GetAllDealersAsync()
        {
            try
            {
                var dealers = await _dealerRepository.GetAllDealersAsync();
                var responses = dealers.Select(d => MapToDealerResponse(d)).ToList();
                return Result<IEnumerable<DealerResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DealerResponse>>.InternalServerError($"Error retrieving dealers: {ex.Message}");
            }
        }

        public async Task<Result<DealerResponse>> GetDealerByIdAsync(Guid id)
        {
            try
            {
                var dealer = await _dealerRepository.GetDealerByIdAsync(id);
                if (dealer == null)
                {
                    return Result<DealerResponse>.NotFound($"Dealer with ID {id} not found.");
                }
                return Result<DealerResponse>.Success(MapToDealerResponse(dealer));
            }
            catch (Exception ex)
            {
                return Result<DealerResponse>.InternalServerError($"Error retrieving dealer: {ex.Message}");
            }
        }

        // --- UPDATE LOGIC ---
        public async Task<Result<DealerResponse>> UpdateDealerAsync(Guid id, DealerRequest request)
        {
            try
            {
                var dealer = await _dealerRepository.GetDealerByIdAsync(id);
                if (dealer == null)
                {
                    return Result<DealerResponse>.NotFound($"Dealer with ID {id} not found.");
                }

                if (request.Name != null) dealer.Name = request.Name;
                if (request.Address != null) dealer.Address = request.Address;
                if (request.Phone != null) dealer.Phone = request.Phone;
                if (request.Email != null) dealer.Email = request.Email;
                if (request.ContractNumber != null) dealer.ContractNumber = request.ContractNumber;
                if (request.SalesTarget.HasValue) dealer.SalesTarget = request.SalesTarget.Value;
                if (request.IsActive.HasValue) dealer.IsActive = request.IsActive.Value;

                dealer.UpdatedAt = TimeHelper.GetVietNamTime();

                await _dealerRepository.UpdateDealerAsync(dealer);

                // Lấy lại để có Evm navigation property đầy đủ
                var updatedDealer = await _dealerRepository.GetDealerByIdAsync(dealer.Id);

                return Result<DealerResponse>.Success(MapToDealerResponse(updatedDealer), "Dealer updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<DealerResponse>.InternalServerError($"Error updating dealer: {ex.Message}");
            }
        }

        // --- DELETE LOGIC ---
        public async Task<Result<bool>> DeleteDealerAsync(Guid id)
        {
            try
            {
                var dealer = await _dealerRepository.GetDealerByIdAsync(id);
                if (dealer == null)
                {
                    return Result<bool>.NotFound($"Dealer with ID {id} not found.");
                }

                // Logic nghiệp vụ: Cần kiểm tra xem có User nào đang gán cho Dealer này không, v.v.

                await _dealerRepository.DeleteDealerAsync(id);
                return Result<bool>.Success(true, "Dealer deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting dealer: {ex.Message}");
            }
        }
    }
}
