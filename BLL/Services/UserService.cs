using AutoMapper;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using BCrypt.Net; 

namespace Common_BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // --- AUTH & CREATE ---
        public async Task<UserResponseDto> RegisterUserAsync(UserCreateRequestDto request)
        {
            // 1. Kiểm tra tồn tại
            if (await _userRepository.GetUserByUsernameAsync(request.UserName) != null ||
                await _userRepository.GetUserByUsernameAsync(request.Email!) != null)
            {
                throw new Exception("Username or Email already exists.");
            }

            // 2. Ánh xạ và Hash mật khẩu
            var userEntity = _mapper.Map<User>(request);
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            userEntity.Role = "Customer"; // Thiết lập Role mặc định
            userEntity.KycStatus = "Pending"; // Thiết lập trạng thái KYC mặc định

            // 3. Gọi DAL để tạo
            var newUser = await _userRepository.CreateUserAsync(userEntity);

            return _mapper.Map<UserResponseDto>(newUser);
        }

        public async Task<UserResponseDto?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            // 1. Kiểm tra User tồn tại và mật khẩu
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; // Xác thực thất bại
            }

            // 2. Kiểm tra tài khoản bị khóa
            if (user.IsLocked)
            {
                throw new Exception("Account is locked.");
            }

            // 3. Trả về DTO (loại bỏ Hash)
            return _mapper.Map<UserResponseDto>(user);
        }

        // --- READ ---
        public async Task<UserResponseDto?> GetUserByIdAsync(Guid userId)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);
            return _mapper.Map<UserResponseDto>(userEntity);
        }

        public async Task<(List<UserResponseDto> Users, int TotalCount)> GetUsersAsync(UserFilterDto filter)
        {
            var (users, totalCount) = await _userRepository.GetAllUsersAsync(filter);
            var userDtos = _mapper.Map<List<UserResponseDto>>(users);

            return (userDtos, totalCount);
        }

        // --- UPDATE & DELETE ---
        public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UserUpdateRequestDto request)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Ánh xạ các trường được phép cập nhật từ Request DTO vào Entity hiện tại
            _mapper.Map(request, existingUser);

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);

            return _mapper.Map<UserResponseDto>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            // Trong thực tế, nên kiểm tra quyền hạn trước khi xóa
            return await _userRepository.DeleteUserAsync(userId);
        }

        // --- ADMIN TASK ---
        public async Task<UserResponseDto> UpdateKycStatusAsync(Guid userId, string newKycStatus)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            existingUser.KycStatus = newKycStatus;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);

            return _mapper.Map<UserResponseDto>(updatedUser);
        }
    }
}