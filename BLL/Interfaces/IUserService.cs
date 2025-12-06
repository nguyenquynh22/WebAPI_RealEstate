// Common.BLL/Interfaces/IUserService.cs

using Common_DTOs.DTOs;
using Common_DTOs.Entities;
using System.Threading.Tasks;

namespace Common_BLL.Interfaces
{
    public interface IUserService
    {
        // Đăng ký và Xác thực
        Task<UserResponseDto> RegisterUserAsync(UserCreateRequestDto request);
        Task<UserResponseDto?> AuthenticateAsync(string username, string password);

        // CRUD
        Task<UserResponseDto?> GetUserByIdAsync(Guid userId);
        Task<(List<UserResponseDto> Users, int TotalCount)> GetUsersAsync(UserFilterDto filter);
        Task<UserResponseDto> UpdateUserAsync(Guid userId, UserUpdateRequestDto request);
        Task<bool> DeleteUserAsync(Guid userId);

        // Cập nhật KYC
        Task<UserResponseDto> UpdateKycStatusAsync(Guid userId, string newKycStatus);
    }
}