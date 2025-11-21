using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        public CustomerService(ICustomerRepository repo) { _repo = repo; }

        public async Task<Result<CustomerResponse>> CreateAsync(CustomerCreateRequest request, Guid dealerStaffId)
        {
            var existing = await _repo.GetByPhoneAsync(request.Phone);
            if (existing != null)
                return Result<CustomerResponse>.Conflict("Customer with this phone already exists.");

            var c = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                DealerStaffId = dealerStaffId,
                CreatedAt = TimeHelper.GetVietNamTime()
            };

            var created = await _repo.AddAsync(c);
            var resp = new CustomerResponse
            {
                Id = created.Id,
                FullName = created.FullName,
                Phone = created.Phone,
                Email = created.Email,
                Address = created.Address,
                CreatedAt = created.CreatedAt
            };
            return Result<CustomerResponse>.Success(resp, "Customer created");
        }

        public async Task<Result<CustomerResponse>> GetByIdAsync(Guid id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return Result<CustomerResponse>.NotFound("Customer not found");
            var resp = new CustomerResponse
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            };
            return Result<CustomerResponse>.Success(resp);
        }

        public async Task<Result<IEnumerable<CustomerResponse>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var mapped = list.Select(c => new CustomerResponse
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
            return Result<IEnumerable<CustomerResponse>>.Success(mapped);
        }

        public async Task<Result<CustomerResponse>> UpdateAsync(Guid id, CustomerUpdateRequest request)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return Result<CustomerResponse>.NotFound("Customer not found");

            c.FullName = request.FullName ?? c.FullName;
            c.Phone = request.Phone ?? c.Phone;
            c.Email = request.Email ?? c.Email;
            c.Address = request.Address ?? c.Address;
            c.UpdatedAt = TimeHelper.GetVietNamTime();

            var updated = await _repo.UpdateAsync(c);
            var resp = new CustomerResponse
            {
                Id = updated.Id,
                FullName = updated.FullName,
                Phone = updated.Phone,
                Email = updated.Email,
                Address = updated.Address,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };
            return Result<CustomerResponse>.Success(resp, "Customer updated");
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var ok = await _repo.DeleteAsync(id);
            if (!ok) return Result<bool>.NotFound("Customer not found");
            return Result<bool>.Success(true, "Customer deleted");
        }

        public async Task<Result<CustomerResponse>> GetByPhoneAsync(string phone)
        {
            var c = await _repo.GetByPhoneAsync(phone);
            if (c == null) return Result<CustomerResponse>.NotFound("Customer not found");
            var resp = new CustomerResponse
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            };
            return Result<CustomerResponse>.Success(resp);
        }
    }
}
