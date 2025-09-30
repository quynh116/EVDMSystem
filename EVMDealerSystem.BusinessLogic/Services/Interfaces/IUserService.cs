using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.User;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync();
        Task<Result<UserResponse>> GetUserByIdAsync(Guid id);
        Task<Result<UserResponse>> UpdateUserAsync(Guid id, UserUpdateRequest request);
        Task<Result<bool>> DeleteUserAsync(Guid id);
        Task<Result<LoginResponse>> LoginAsync(UserLoginRequest request);
        Task<Result<UserResponse>> RegisterUserByAdminAsync(AdminUserRegistrationRequest request);
    }
}
