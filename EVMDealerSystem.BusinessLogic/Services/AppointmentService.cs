using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
//using EVMDealerSystem.BusinessLogic.Models.Request.Appointment;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IInventoryRepository _inventoryRepo;

        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            ICustomerRepository customerRepo,
            IInventoryRepository inventoryRepo)
        {
            _appointmentRepo = appointmentRepo;
            _customerRepo = customerRepo;
            _inventoryRepo = inventoryRepo;
        }

        public async Task<Result<AppointmentResponse>> CreateAsync(AppointmentCreateRequest request, Guid dealerStaffId)
        {
            try
            {
                if (request == null)
                    return Result<AppointmentResponse>.Invalid("Invalid request.");

                if (request.VehicleId == Guid.Empty || request.DealerId == Guid.Empty)
                    return Result<AppointmentResponse>.Invalid("VehicleId and DealerId are required.");

                Customer? customer = null;

                if (!string.IsNullOrWhiteSpace(request.NewCustomer.Phone))
                {
                    string phone = request.NewCustomer.Phone.Trim();

                    customer = await _customerRepo.GetByPhoneAsync(phone);

                    if (customer == null)
                    {
                        var newCustomer = new Customer
                        {
                            Id = Guid.NewGuid(),
                            FullName = request.NewCustomer?.FullName ?? "Unnamed",
                            Phone = phone,
                            Email = request.NewCustomer?.Email,
                            Address = request.NewCustomer?.Address,
                            DealerStaffId = dealerStaffId,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _customerRepo.AddAsync(newCustomer);
                        customer = newCustomer;
                    }
                }
                else
                {
                    return Result<AppointmentResponse>.Invalid("Customer phone or ID is required.");
                }

                var invQuery = await _inventoryRepo.GetInventoryQueryAsync();
                var inventory = await invQuery
                    .Include(i => i.Vehicle)
                    .Include(i => i.Dealer)
                    .FirstOrDefaultAsync(i => i.VehicleId == request.VehicleId && i.DealerId == request.DealerId);

                if (inventory == null)
                    return Result<AppointmentResponse>.NotFound("Vehicle not available at this dealer.");

                var existingAppointments = await _appointmentRepo.GetByVehicleAndDateAsync(request.VehicleId, request.AppointmentDate);
                bool isConflict = existingAppointments.Any(a => a.AppointmentDate == request.AppointmentDate);

                if (isConflict)
                    return Result<AppointmentResponse>.Conflict("The selected time slot is already booked for this vehicle.");

                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    DealerStaffId = dealerStaffId,
                    VehicleId = request.VehicleId,
                    AppointmentDate = request.AppointmentDate,
                    Status = "Booked",
                    Note = request.Note,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _appointmentRepo.AddAsync(appointment);

                var response = Map(created);
                return Result<AppointmentResponse>.Success(response, "Appointment created successfully.");
            }
            catch (Exception ex)
            {
                return Result<AppointmentResponse>.InternalServerError($"Failed to create appointment: {ex.Message}");
            }
        }


        public async Task<Result<AppointmentResponse>> GetByIdAsync(Guid id)
        {
            var a = await _appointmentRepo.GetByIdAsync(id);
            if (a == null) return Result<AppointmentResponse>.NotFound("Appointment not found");
            return Result<AppointmentResponse>.Success(Map(a));
        }

        public async Task<Result<IEnumerable<AppointmentResponse>>> GetAllAsync()
        {
            var list = await _appointmentRepo.GetAllAsync();
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<AppointmentResponse>>.Success(mapped);
        }

        public async Task<Result<AppointmentResponse>> UpdateAsync(Guid id, AppointmentUpdateRequest request)
        {
            var a = await _appointmentRepo.GetByIdAsync(id);
            if (a == null) return Result<AppointmentResponse>.NotFound("Appointment not found");

            a.AppointmentDate = request.AppointmentDate ?? a.AppointmentDate;
            a.Status = request.Status ?? a.Status;
            a.Note = request.Note ?? a.Note;
            a.UpdatedAt = DateTime.UtcNow;

            var updated = await _appointmentRepo.UpdateAsync(a);
            return Result<AppointmentResponse>.Success(Map(updated), "Appointment updated");
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var ok = await _appointmentRepo.DeleteAsync(id);
            if (!ok) return Result<bool>.NotFound("Appointment not found");
            return Result<bool>.Success(true, "Appointment deleted");
        }

        public async Task<Result<IEnumerable<AppointmentResponse>>> GetByDealerIdAsync(Guid dealerStaffId)
        {
            var list = await _appointmentRepo.GetByDealerIdAsync(dealerStaffId);
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<AppointmentResponse>>.Success(mapped);
        }

        public async Task<Result<IEnumerable<AppointmentResponse>>> GetByVehicleDateAsync(Guid vehicleId, DateTime date)
        {
            var list = await _appointmentRepo.GetByVehicleAndDateAsync(vehicleId, date);
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<AppointmentResponse>>.Success(mapped);
        }

        public async Task<Result<IEnumerable<DateTime>>> GetAvailableSlotsAsync(Guid vehicleId, DateTime date)
        {
            // Example: slots each hour 9..17
            var start = date.Date.AddHours(9);
            var slots = Enumerable.Range(0, 9).Select(i => start.AddHours(i)).ToList();
            var booked = (await _appointmentRepo.GetByVehicleAndDateAsync(vehicleId, date)).Select(a => a.AppointmentDate).ToHashSet();
            var available = slots.Where(s => !booked.Contains(s)).ToList();
            return Result<IEnumerable<DateTime>>.Success(available);
        }

        private AppointmentResponse Map(Appointment a) => new AppointmentResponse
        {
            Id = a.Id,
            CustomerId = a.CustomerId,
            DealerStaffId = a.DealerStaffId,
            VehicleId = a.VehicleId,
            AppointmentDate = a.AppointmentDate,
            Status = a.Status,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            Note = a.Note
        };
    }
}
