//using Common_DAL.Helpers;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Common_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlHelper _sqlHelper;

        public UserRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        // Phương thức hỗ trợ ánh xạ (Mapper)
        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetGuid("UserId"),
                UserName = reader.GetString("UserName"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                Role = reader.GetString("Role"),
                // Xử lý Nullability: Dùng ?.GetString() an toàn hơn, nhưng dùng IsDBNull cũng được
                Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
                Address = reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                AvatarUrl = reader.IsDBNull("AvatarUrl") ? null : reader.GetString("AvatarUrl"),
                Bio = reader.IsDBNull("Bio") ? null : reader.GetString("Bio"),
                IdentityDocumentUrl = reader.IsDBNull("IdentityDocumentUrl") ? null : reader.GetString("IdentityDocumentUrl"),

                IsLocked = reader.GetBoolean("IsLocked"),
                KycStatus = reader.GetString("KycStatus"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                UpdatedAt = reader.IsDBNull("UpdatedAt") ? (DateTime?)null : reader.GetDateTime("UpdatedAt")
            };
        }

        // [1. READ BY ID]
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            const string sql = "SELECT * FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return MapUser(reader);
                }
            }
            return null;
        }

        // [2. READ BY USERNAME]
        public async Task<User?> GetUserByUsernameAsync(string userName)
        {
            const string sql = "SELECT * FROM Users WHERE UserName = @UserName OR Email = @UserName";
            var parameters = new SqlParameter[] { new SqlParameter("@UserName", userName) };

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return MapUser(reader);
                }
            }
            return null;
        }

        // [3. READ ALL (Phân trang và Tìm kiếm) - GIỮ LẠI LOGIC HIỆU QUẢ CỦA BẠN]
        public async Task<(List<User> Users, int TotalCount)> GetAllUsersAsync(UserFilterDto filter)
        {
            var users = new List<User>();
            int totalCount = 0;

            // Xây dựng SQL động cho tìm kiếm và lọc
            var whereClause = "1=1";
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(filter.Role))
            {
                whereClause += " AND Role = @Role";
                parameters.Add(new SqlParameter("@Role", filter.Role));
            }

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                whereClause += " AND (UserName LIKE @Keyword OR Email LIKE @Keyword OR Phone LIKE @Keyword)";
                parameters.Add(new SqlParameter("@Keyword", $"%{filter.Keyword}%"));
            }

            // SQL thực hiện 2 tác vụ: Đếm tổng số và Lấy dữ liệu phân trang
            string sql = $@"
                SELECT COUNT(UserId) FROM Users WHERE {whereClause};
                
                SELECT * FROM Users
                WHERE {whereClause}
                ORDER BY CreatedAt DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY;";

            parameters.Add(new SqlParameter("@PageSize", filter.PageSize));
            parameters.Add(new SqlParameter("@Offset", filter.PageSize * (filter.PageIndex - 1)));

            // Thực thi 2 câu lệnh với một Reader (Duy trì logic tự mở/đóng kết nối của bạn cho Multi-query)
            using (var connection = _sqlHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // 1. Đọc TotalCount
                        if (await reader.ReadAsync())
                        {
                            totalCount = reader.GetInt32(0);
                        }

                        // 2. Chuyển sang Result Set tiếp theo và đọc Users
                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            return (users, totalCount);
        }

        // [4. CREATE]
        public async Task<User> CreateUserAsync(User user)
        {
            const string sql = @"
                INSERT INTO Users (UserId, UserName, Email, PasswordHash, Role, Phone, Address, AvatarUrl, Bio, IdentityDocumentUrl, IsLocked, KycStatus, CreatedAt) 
                VALUES (@UserId, @UserName, @Email, @PasswordHash, @Role, @Phone, @Address, @AvatarUrl, @Bio, @IdentityDocumentUrl, @IsLocked, @KycStatus, @CreatedAt)";

            user.UserId = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@Phone", (object?)user.Phone ?? DBNull.Value),
                new SqlParameter("@Address", (object?)user.Address ?? DBNull.Value),
                new SqlParameter("@AvatarUrl", (object?)user.AvatarUrl ?? DBNull.Value),
                new SqlParameter("@Bio", (object?)user.Bio ?? DBNull.Value),
                new SqlParameter("@IdentityDocumentUrl", (object?)user.IdentityDocumentUrl ?? DBNull.Value),
                new SqlParameter("@IsLocked", user.IsLocked),
                new SqlParameter("@KycStatus", user.KycStatus),
                new SqlParameter("@CreatedAt", user.CreatedAt)
            };

            await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return user;
        }

        // [5. UPDATE]
        public async Task<User> UpdateUserAsync(User user)
        {
            const string sql = @"
                UPDATE Users SET
                    UserName = @UserName,
                    Email = @Email,
                    PasswordHash = @PasswordHash, 
                    Role = @Role,
                    Phone = @Phone,
                    Address = @Address,
                    AvatarUrl = @AvatarUrl,
                    Bio = @Bio,
                    IsLocked = @IsLocked,
                    KycStatus = @KycStatus,
                    UpdatedAt = @UpdatedAt
                WHERE UserId = @UserId";

            user.UpdatedAt = DateTime.UtcNow;

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@Phone", (object?)user.Phone ?? DBNull.Value),
                new SqlParameter("@Address", (object?)user.Address ?? DBNull.Value),
                new SqlParameter("@AvatarUrl", (object?)user.AvatarUrl ?? DBNull.Value),
                new SqlParameter("@Bio", (object?)user.Bio ?? DBNull.Value),
                new SqlParameter("@IsLocked", user.IsLocked),
                new SqlParameter("@KycStatus", user.KycStatus),
                new SqlParameter("@UpdatedAt", user.UpdatedAt)
            };

            await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return user;
        }

        // [6. DELETE]
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            const string sql = "DELETE FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", id) };

            var rowsAffected = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return rowsAffected > 0;
        }
    }
}