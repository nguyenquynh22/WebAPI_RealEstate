// File: Common_DAL/Interfaces/IUserRepository.cs (ĐÃ SỬA)

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.Entities;
using Common_DTOs.DTOs;

namespace Common_DAL.Interfaces
{
    // PHẢI dùng từ khóa interface
    public interface IUserRepository
    {
        // PHẢI kết thúc bằng dấu chấm phẩy (;)
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByUsernameAsync(string userName); // Đổi tên tham số cho rõ ràng
        Task<(List<User> Users, int TotalCount)> GetAllUsersAsync(UserFilterDto filter);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        // Lưu ý: Đổi tên thành DeleteUserAsync(Guid id) cho đúng với IUserRepository bạn muốn
        Task<bool> DeleteUserAsync(Guid id);
    }
}