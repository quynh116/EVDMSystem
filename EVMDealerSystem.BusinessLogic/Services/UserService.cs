using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.User;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.BusinessLogic.Token;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ProvideToken _provideToken;

        public UserService(IUserRepository userRepository, ProvideToken provideToken) 
        {
            _userRepository = userRepository;
            _provideToken = provideToken;
        }

        private UserResponse MapToUserResponse(User user)
        {
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? "N/A",
                DealerId = user.DealerId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt

            };
        }

        public async Task<Result<bool>> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Result<bool>.NotFound($"User with ID {id} not found.");
                }

                await _userRepository.DeleteUserAsync(id);
                return Result<bool>.Success(true, "User deleted.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting user: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                var userResponses = users.Select(u => MapToUserResponse(u)).ToList();
                return Result<IEnumerable<UserResponse>>.Success(userResponses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserResponse>>.InternalServerError($"Error retrieving users: {ex.Message}");
            }
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Result<UserResponse>.NotFound($"User with ID {id} not found.");
                }
                return Result<UserResponse>.Success(MapToUserResponse(user));
            }
            catch (Exception ex)
            {
                return Result<UserResponse>.InternalServerError($"Error retrieving user: {ex.Message}");
            }
        }

        public async Task<Result<LoginResponse>> LoginAsync(UserLoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return Result<LoginResponse>.NotFound("Invalid credentials");
                }
                if (request.Password != user.Password) 
                { return Result<LoginResponse>.Invalid("Invalid credentials"); }
                if (!user.IsActive)
                {
                    
                    return Result<LoginResponse>.Unauthorized("Your account has been locked.");
                }
                var token = _provideToken.GenerateToken(user);
                var tokenExpires = _provideToken.GetTokenExpirationTime();
                var loginResponse = new LoginResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role?.Name ?? "",
                    Token = token,
                    TokenExpires = tokenExpires
                };
                return Result<LoginResponse>.Success(loginResponse, "Login successful.");
            }
            catch (Exception ex)
            {
                
                return Result<LoginResponse>.InternalServerError($"An unexpected error occurred during login: {ex.Message}");
            }
        }

        public async Task<Result<UserResponse>> RegisterUserByAdminAsync(AdminUserRegistrationRequest request)
        {
            try
            {
                if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
                {
                    return Result<UserResponse>.Conflict("Email already exists.");
                }
                var targetRole = await _userRepository.GetRoleByNameAsync(request.RoleName);
                if (targetRole == null)
                {
                    return Result<UserResponse>.NotFound($"Role '{request.RoleName}' not found.");
                }
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = request.Password,
                    RoleId = targetRole.Id,
                    DealerId = request.DealerId,
                    IsActive = request.IsActive,
                    CreatedAt = TimeHelper.GetVietNamTime(),
                };
                var addedUser = await _userRepository.AddUserAsync(newUser);
                var userWithRole = await _userRepository.GetUserByIdAsync(addedUser.Id);
                return Result<UserResponse>.Success(MapToUserResponse(userWithRole), $"{request.RoleName} registered successfully.");
            }
            catch (Exception ex)
            {
                return Result<UserResponse>.InternalServerError($"Error retrieving user: {ex.Message}");
            }
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(Guid id, UserUpdateRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Result<UserResponse>.NotFound($"User with ID {id} not found.");
                }
                if (request.IsActive.HasValue)
                {
                    user.IsActive = request.IsActive.Value;
                }

                if (request.RoleId.HasValue)
                {
                    user.RoleId = request.RoleId.Value;
                }
                if (request.DealerId.HasValue)
                {
                    user.DealerId = request.DealerId.Value;
                }
                user.UpdatedAt = TimeHelper.GetVietNamTime();
                await _userRepository.UpdateUserAsync(user);
                var updatedUserWithRole = await _userRepository.GetUserByIdAsync(user.Id);
                return Result<UserResponse>.Success(MapToUserResponse(updatedUserWithRole), "User updated.");
            }
            catch (Exception ex)
            {
                return Result<UserResponse>.InternalServerError($"Error retrieving user: {ex.Message}");
            }
        }
    }
}
