using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
//using EVMDealerSystem.BusinessLogic.Models.Request.Appointment;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<AppointmentResponse>> CreateAsync(AppointmentCreateRequest request, Guid dealerStaffId);
        Task<Result<AppointmentResponse>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<AppointmentResponse>>> GetAllAsync();
        //Task<Result<AppointmentResponse>> UpdateAsync(Guid id, AppointmentUpdateRequest request);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<IEnumerable<AppointmentResponse>>> GetByDealerIdAsync(Guid dealerStaffId);
        Task<Result<IEnumerable<AppointmentResponse>>> GetByVehicleDateAsync(Guid vehicleId, DateTime date);
        Task<Result<IEnumerable<DateTime>>> GetAvailableSlotsAsync(Guid vehicleId, DateTime date);
    }
}
