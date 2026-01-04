using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Common_DAL.Repositories
{
    public class ConversationsRepository : IConversationsRepository
    {
        private readonly SqlHelper _sqlHelper;

        public ConversationsRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        // Tạo ConversationId theo mẫu: GUID1_GUID2 (luôn sắp xếp để tránh trùng đảo chiều)
        private string BuildConversationId(Guid a, Guid b)
        {
            string sa = a.ToString();
            string sb = b.ToString();
            return string.CompareOrdinal(sa, sb) <= 0 ? $"{sa}_{sb}" : $"{sb}_{sa}";
        }

        private void NormalizeParticipants(Guid a, Guid b, out Guid p1, out Guid p2)
        {
            string sa = a.ToString();
            string sb = b.ToString();
            if (string.CompareOrdinal(sa, sb) <= 0)
            {
                p1 = a; p2 = b;
            }
            else
            {
                p1 = b; p2 = a;
            }
        }

        public async Task<string> CreateOrGetAsync(ConversationsCreateRequestDto dto)
        {
            NormalizeParticipants(dto.Participant1Id, dto.Participant2Id, out var p1, out var p2);
            string conversationId = BuildConversationId(dto.Participant1Id, dto.Participant2Id);

            // Nếu tồn tại -> trả về luôn
            string sqlExist = @"
SELECT ConversationId
FROM dbo.Conversations
WHERE Participant1Id=@P1 AND Participant2Id=@P2;
";
            object exist = await _sqlHelper.ExecuteScalarAsync(sqlExist, CommandType.Text,
                new SqlParameter("@P1", p1),
                new SqlParameter("@P2", p2));

            if (exist != null && exist != DBNull.Value)
                return exist.ToString()!;

            // Chưa có -> tạo mới
            string sqlInsert = @"
INSERT INTO dbo.Conversations (ConversationId, Participant1Id, Participant2Id)
VALUES (@Id, @P1, @P2);
";
            await _sqlHelper.ExecuteNonQueryAsync(sqlInsert, CommandType.Text,
                new SqlParameter("@Id", conversationId),
                new SqlParameter("@P1", p1),
                new SqlParameter("@P2", p2));

            return conversationId;
        }

        public async Task<ConversationsResponseDto> GetByIdAsync(string conversationId)
        {
            string sql = @"
SELECT ConversationId, Participant1Id, Participant2Id, LastMessageAt, CreatedAt, UpdatedAt
FROM dbo.Conversations
WHERE ConversationId = @Id;
";

            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text,
                new SqlParameter("@Id", conversationId));

            if (!await reader.ReadAsync()) return null;

            return new ConversationsResponseDto
            {
                ConversationId = reader.GetString(0),
                Participant1Id = reader.GetGuid(1),
                Participant2Id = reader.GetGuid(2),
                LastMessageAt = reader.GetDateTime(3),
                CreatedAt = reader.GetDateTime(4),
                UpdatedAt = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5)
            };
        }

        public async Task<(List<ConversationsResponseDto> Items, int Total)> GetListAsync(ConversationsFilterDto filter)
        {
            int page = filter.Page <= 0 ? 1 : filter.Page;
            int pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;
            int offset = (page - 1) * pageSize;

            string sqlItems = @"
SELECT ConversationId, Participant1Id, Participant2Id, LastMessageAt, CreatedAt, UpdatedAt
FROM dbo.Conversations
WHERE (@UserId IS NULL OR Participant1Id=@UserId OR Participant2Id=@UserId)
ORDER BY LastMessageAt DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
";

            string sqlCount = @"
SELECT COUNT(1)
FROM dbo.Conversations
WHERE (@UserId IS NULL OR Participant1Id=@UserId OR Participant2Id=@UserId);
";

            object userIdParam = filter.UserId == Guid.Empty ? DBNull.Value : filter.UserId;

            var p = new SqlParameter[]
            {
                new SqlParameter("@UserId", userIdParam),
                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize),
            };

            var items = new List<ConversationsResponseDto>();

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sqlItems, CommandType.Text, p))
            {
                while (await reader.ReadAsync())
                {
                    items.Add(new ConversationsResponseDto
                    {
                        ConversationId = reader.GetString(0),
                        Participant1Id = reader.GetGuid(1),
                        Participant2Id = reader.GetGuid(2),
                        LastMessageAt = reader.GetDateTime(3),
                        CreatedAt = reader.GetDateTime(4),
                        UpdatedAt = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5)
                    });
                }
            }

            int total = Convert.ToInt32(await _sqlHelper.ExecuteScalarAsync(sqlCount, CommandType.Text, p));
            return (items, total);
        }

        public async Task<bool> DeleteAsync(string conversationId)
        {
            // Messages có ON DELETE CASCADE theo SQL của bạn => xóa Conversation sẽ tự xóa Messages
            string sql = "DELETE FROM dbo.Conversations WHERE ConversationId=@Id;";
            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text,
                new SqlParameter("@Id", conversationId));
            return rows > 0;
        }
    }
}
