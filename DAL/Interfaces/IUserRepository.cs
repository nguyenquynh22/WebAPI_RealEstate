using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.Entities;
using Common_DTOs.DTOs;

namespace Common_DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByUsernameAsync(string userName); 
        Task<(List<User> Users, int TotalCount)> GetAllUsersAsync(UserFilterDto filter);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}