using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Common_DAL.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly SqlHelper _sqlHelper;

        public CommentsRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        private object DbGuid(Guid v) => v == Guid.Empty ? DBNull.Value : v;

        public async Task<Guid> CreateAsync(CommentsCreateRequestDto dto)
        {
            //  VALIDATE đúng constraint CHK_Comment_OneTarget trong SQL:
            // (NewsId is null) + (ListingId is null) = 1  => chỉ được chọn 1 target
            bool hasNews = dto.NewsId != Guid.Empty;
            bool hasListing = dto.ListingId != Guid.Empty;

            if (hasNews == hasListing) // cả 2 true hoặc cả 2 false
            {
                throw new Exception("Comment phải thuộc News HOẶC Listing (chỉ 1 trong 2).");
            }

            // SQL của bạn: CreatedAt có DEFAULT rồi => không cần truyền CreatedAt
            string sql = @"
INSERT INTO dbo.Comments
(NewsId, ListingId, UserId, Content, ParentId)
OUTPUT INSERTED.CommentId
VALUES
(@NewsId, @ListingId, @UserId, @Content, @ParentId);
";

            object idObj = await _sqlHelper.ExecuteScalarAsync(sql, CommandType.Text,
                new SqlParameter("@NewsId", DbGuid(dto.NewsId)),
                new SqlParameter("@ListingId", DbGuid(dto.ListingId)),
                new SqlParameter("@UserId", dto.UserId),
                new SqlParameter("@Content", dto.Content),
                new SqlParameter("@ParentId", DbGuid(dto.ParentId))
            );

            return (Guid)idObj;
        }

        public async Task<CommentsResponseDto> GetByIdAsync(Guid commentId)
        {
            // thêm LikesCount theo SQL
            string sql = @"
SELECT CommentId, NewsId, ListingId, UserId, Content, ParentId, LikesCount, CreatedAt, UpdatedAt
FROM dbo.Comments
WHERE CommentId = @Id;
";

            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text,
                new SqlParameter("@Id", commentId));

            if (!await reader.ReadAsync()) return null;

            return Map(reader);
        }

        public async Task<bool> UpdateAsync(CommentsUpdateRequestDto dto)
        {
            string sql = @"
UPDATE dbo.Comments
SET Content = @Content,
    UpdatedAt = SYSUTCDATETIME()
WHERE CommentId = @Id;
";

            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text,
                new SqlParameter("@Id", dto.CommentId),
                new SqlParameter("@Content", dto.Content));

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid commentId)
        {
            string sql = "DELETE FROM dbo.Comments WHERE CommentId = @Id;";
            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text,
                new SqlParameter("@Id", commentId));
            return rows > 0;
        }

        public async Task<(List<CommentsResponseDto> Items, int Total)> GetListAsync(CommentsFilterDto filter)
        {
            int page = filter.Page <= 0 ? 1 : filter.Page;
            int pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;
            int offset = (page - 1) * pageSize;

            //  thêm LikesCount theo SQL
            string sqlItems = @"
SELECT CommentId, NewsId, ListingId, UserId, Content, ParentId, LikesCount, CreatedAt, UpdatedAt
FROM dbo.Comments
WHERE 1=1
  AND (@NewsId IS NULL OR NewsId = @NewsId)
  AND (@ListingId IS NULL OR ListingId = @ListingId)
ORDER BY CreatedAt DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
";

            string sqlCount = @"
SELECT COUNT(1)
FROM dbo.Comments
WHERE 1=1
  AND (@NewsId IS NULL OR NewsId = @NewsId)
  AND (@ListingId IS NULL OR ListingId = @ListingId);
";

            var p = new SqlParameter[]
            {
                new SqlParameter("@NewsId", DbGuid(filter.NewsId)),
                new SqlParameter("@ListingId", DbGuid(filter.ListingId)),
                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize),
            };

            var items = new List<CommentsResponseDto>();

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sqlItems, CommandType.Text, p))
            {
                while (await reader.ReadAsync())
                {
                    items.Add(Map(reader));
                }
            }

            int total = Convert.ToInt32(await _sqlHelper.ExecuteScalarAsync(sqlCount, CommandType.Text, p));
            return (items, total);
        }

        private CommentsResponseDto Map(SqlDataReader r)
        {
            return new CommentsResponseDto
            {
                CommentId = r.GetGuid(r.GetOrdinal("CommentId")),
                NewsId = r.IsDBNull(r.GetOrdinal("NewsId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("NewsId")),
                ListingId = r.IsDBNull(r.GetOrdinal("ListingId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ListingId")),
                UserId = r.GetGuid(r.GetOrdinal("UserId")),
                Content = r.GetString(r.GetOrdinal("Content")),
                ParentId = r.IsDBNull(r.GetOrdinal("ParentId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ParentId")),
                LikesCount = r.IsDBNull(r.GetOrdinal("LikesCount")) ? 0 : r.GetInt32(r.GetOrdinal("LikesCount")),
                CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt")),
                UpdatedAt = r.IsDBNull(r.GetOrdinal("UpdatedAt")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("UpdatedAt")),
            };
        }
    }
}
