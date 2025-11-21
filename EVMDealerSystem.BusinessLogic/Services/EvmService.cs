using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
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
    public class EvmService : IEvmService
    {
        private readonly IEvmRepository _evmRepository;

        public EvmService(IEvmRepository evmRepository)
        {
            _evmRepository = evmRepository;
        }

        private EvmResponse MapToEvmResponse(Evm evm)
        {
            if (evm == null) return null;

            return new EvmResponse
            {
                Id = evm.Id,
                Name = evm.Name,
                Address = evm.Address,
                Phone = evm.Phone,
                Email = evm.Email,
                CreatedAt = evm.CreatedAt,
                UpdatedAt = evm.UpdatedAt
            };
        }

        public async Task<Result<IEnumerable<EvmResponse>>> GetAllEvmsAsync()
        {
            try
            {
                var evms = await _evmRepository.GetAllEvmsAsync();
                var evmResponses = evms.Select(e => MapToEvmResponse(e)).ToList();
                return Result<IEnumerable<EvmResponse>>.Success(evmResponses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<EvmResponse>>.InternalServerError($"Error retrieving EVMs: {ex.Message}");
            }
        }

        public async Task<Result<EvmResponse>> GetEvmByIdAsync(Guid id)
        {
            try
            {
                var evm = await _evmRepository.GetEvmByIdAsync(id);
                if (evm == null)
                {
                    return Result<EvmResponse>.NotFound($"EVM with ID {id} not found.");
                }
                return Result<EvmResponse>.Success(MapToEvmResponse(evm));
            }
            catch (Exception ex)
            {
                return Result<EvmResponse>.InternalServerError($"Error retrieving EVM: {ex.Message}");
            }
        }

        public async Task<Result<EvmResponse>> CreateEvmAsync(EvmRequest request)
        {
            try
            {

                var newEvm = new Evm
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Address = request.Address,
                    Phone = request.Phone,
                    Email = request.Email,
                    CreatedAt = TimeHelper.GetVietNamTime(),
                    UpdatedAt = null
                };

                var addedEvm = await _evmRepository.AddEvmAsync(newEvm);
                return Result<EvmResponse>.Success(MapToEvmResponse(addedEvm), "EVM created successfully.");
            }
            catch (Exception ex)
            {
                return Result<EvmResponse>.InternalServerError($"Error creating EVM: {ex.Message}");
            }
        }

        public async Task<Result<EvmResponse>> UpdateEvmAsync(Guid id, EvmRequest request)
        {
            try
            {
                var evm = await _evmRepository.GetEvmByIdAsync(id);
                if (evm == null)
                {
                    return Result<EvmResponse>.NotFound($"EVM with ID {id} not found.");
                }

                if (request.Name != null) evm.Name = request.Name;
                if (request.Address != null) evm.Address = request.Address;
                if (request.Phone != null) evm.Phone = request.Phone;
                if (request.Email != null) evm.Email = request.Email;

                evm.UpdatedAt = TimeHelper.GetVietNamTime();

                await _evmRepository.UpdateEvmAsync(evm);

                var updatedEvm = await _evmRepository.GetEvmByIdAsync(evm.Id);

                return Result<EvmResponse>.Success(MapToEvmResponse(updatedEvm), "EVM updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<EvmResponse>.InternalServerError($"Error updating EVM: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteEvmAsync(Guid id)
        {
            try
            {
                var evm = await _evmRepository.GetEvmByIdAsync(id);
                if (evm == null)
                {
                    return Result<bool>.NotFound($"EVM with ID {id} not found.");
                }


                await _evmRepository.DeleteEvmAsync(id);
                return Result<bool>.Success(true, "EVM deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting EVM: {ex.Message}");
            }
        }
    }
}
