using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Appointment;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<IEnumerable<AppointmentResponse>>> GetAllAppointmentsAsync();
        Task<Result<AppointmentResponse>> GetAppointmentByIdAsync(Guid id);
        Task<Result<AppointmentResponse>> CreateAppointmentAsync(AppointmentCreateRequest request, Guid dealerStaffId);
        Task<Result<AppointmentResponse>> UpdateAppointmentAsync(Guid id, AppointmentUpdateRequest request);
        Task<Result<bool>> DeleteAppointmentAsync(Guid id);
        Task<Result<IEnumerable<AppointmentResponse>>> GetAppointmentsByStaffAsync(Guid staffId);
    }
}
