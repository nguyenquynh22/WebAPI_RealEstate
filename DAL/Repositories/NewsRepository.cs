using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Common_DAL.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly SqlHelper _sqlHelper;

        public NewsRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        private object DbGuid(Guid v) => v == Guid.Empty ? DBNull.Value : v;
        private object DbString(string v) => string.IsNullOrWhiteSpace(v) ? DBNull.Value : v;

        // -1 => NULL (lọc bỏ), 0/1 => bit
        private object DbBoolFilter(int v)
        {
            if (v == -1) return DBNull.Value;
            return v == 1;
        }

        // -1 => NULL (để DB default), >=0 => value
        private object DbIntDefault(int v) => v < 0 ? DBNull.Value : v;

        public async Task<Guid> CreateAsync(NewsCreateRequestDto dto)
        {
            string sql = @"
INSERT INTO dbo.News
(PosterId, ProjectId, ListingId, Title, Slug, ContentType, ShortDescription,
 Content, ThumbnailUrl, ImagesJson, Tags, Status, Views, IsHighlight, IsExternal)
OUTPUT INSERTED.NewsId
VALUES
(@PosterId, @ProjectId, @ListingId, @Title, @Slug, @ContentType, @ShortDescription,
 @Content, @ThumbnailUrl, @ImagesJson, @Tags, @Status, @Views, @IsHighlight, @IsExternal);
";

            SqlParameter[] p =
            {
                new SqlParameter("@PosterId", dto.PosterId),
                new SqlParameter("@ProjectId", DbGuid(dto.ProjectId)),
                new SqlParameter("@ListingId", DbGuid(dto.ListingId)),
                new SqlParameter("@Title", dto.Title),

                new SqlParameter("@Slug", DbString(dto.Slug)),
                new SqlParameter("@ContentType", DbString(dto.ContentType)),
                new SqlParameter("@ShortDescription", DbString(dto.ShortDescription)),
                new SqlParameter("@Content", DbString(dto.Content)),
                new SqlParameter("@ThumbnailUrl", DbString(dto.ThumbnailUrl)),
                new SqlParameter("@ImagesJson", DbString(dto.ImagesJson)),
                new SqlParameter("@Tags", DbString(dto.Tags)),

                // nếu bạn để "" thì DB default Published
                new SqlParameter("@Status", DbString(dto.Status)),
                // nếu Views=-1 thì DB default 0
                new SqlParameter("@Views", DbIntDefault(dto.Views)),
                // nếu -1 thì DB default 0
                new SqlParameter("@IsHighlight", DbBoolFilter(dto.IsHighlight)),
                new SqlParameter("@IsExternal", DbBoolFilter(dto.IsExternal)),
            };

            object idObj = await _sqlHelper.ExecuteScalarAsync(sql, CommandType.Text, p);
            return (Guid)idObj;
        }

        public async Task<NewsResponseDto> GetByIdAsync(Guid newsId)
        {
            string sql = @"
SELECT NewsId, PosterId, ProjectId, ListingId,
       Title, Slug, ContentType, ShortDescription, Content,
       ThumbnailUrl, ImagesJson, Tags,
       PostedAt, Status, Views, IsHighlight, IsExternal,
       CreatedAt, UpdatedAt
FROM dbo.News
WHERE NewsId = @NewsId;
";
            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text,
                new SqlParameter("@NewsId", newsId));

            if (!await reader.ReadAsync()) return null;

            return Map(reader);
        }

        public async Task<bool> UpdateAsync(NewsUpdateRequestDto dto)
        {
            string sql = @"
UPDATE dbo.News
SET
    Title = @Title,
    Slug = @Slug,
    ContentType = @ContentType,
    ShortDescription = @ShortDescription,
    Content = @Content,
    ThumbnailUrl = @ThumbnailUrl,
    ImagesJson = @ImagesJson,
    Tags = @Tags,
    Status = @Status,
    IsHighlight = CASE WHEN @IsHighlight IS NULL THEN IsHighlight ELSE @IsHighlight END,
    IsExternal  = CASE WHEN @IsExternal  IS NULL THEN IsExternal  ELSE @IsExternal  END,
    UpdatedAt = SYSUTCDATETIME()
WHERE NewsId = @NewsId;
";

            SqlParameter[] p =
            {
                new SqlParameter("@NewsId", dto.NewsId),
                new SqlParameter("@Title", dto.Title),

                new SqlParameter("@Slug", DbString(dto.Slug)),
                new SqlParameter("@ContentType", DbString(dto.ContentType)),
                new SqlParameter("@ShortDescription", DbString(dto.ShortDescription)),
                new SqlParameter("@Content", DbString(dto.Content)),
                new SqlParameter("@ThumbnailUrl", DbString(dto.ThumbnailUrl)),
                new SqlParameter("@ImagesJson", DbString(dto.ImagesJson)),
                new SqlParameter("@Tags", DbString(dto.Tags)),

                // CHECK constraint: Draft/Published/Archived
                new SqlParameter("@Status", DbString(dto.Status)),
                new SqlParameter("@IsHighlight", DbBoolFilter(dto.IsHighlight)),
                new SqlParameter("@IsExternal", DbBoolFilter(dto.IsExternal)),
            };

            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, p);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid newsId)
        {
            string sql = "DELETE FROM dbo.News WHERE NewsId = @NewsId;";
            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text,
                new SqlParameter("@NewsId", newsId));
            return rows > 0;
        }

        public async Task<(List<NewsResponseDto> Items, int Total)> GetListAsync(NewsFilterDto filter)
        {
            int page = filter.Page <= 0 ? 1 : filter.Page;
            int pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;
            int offset = (page - 1) * pageSize;

            string sqlItems = @"
SELECT NewsId, PosterId, ProjectId, ListingId,
       Title, Slug, ContentType, ShortDescription, Content,
       ThumbnailUrl, ImagesJson, Tags,
       PostedAt, Status, Views, IsHighlight, IsExternal,
       CreatedAt, UpdatedAt
FROM dbo.News
WHERE 1=1
  AND (@Status = '' OR Status = @Status)
  AND (@Keyword = '' OR Title LIKE '%' + @Keyword + '%')
  AND (@IsHighlight IS NULL OR IsHighlight = @IsHighlight)
  AND (@IsExternal  IS NULL OR IsExternal  = @IsExternal)
ORDER BY PostedAt DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
";

            string sqlCount = @"
SELECT COUNT(1)
FROM dbo.News
WHERE 1=1
  AND (@Status = '' OR Status = @Status)
  AND (@Keyword = '' OR Title LIKE '%' + @Keyword + '%')
  AND (@IsHighlight IS NULL OR IsHighlight = @IsHighlight)
  AND (@IsExternal  IS NULL OR IsExternal  = @IsExternal);
";

            SqlParameter[] p =
            {
                new SqlParameter("@Status", filter.Status ?? ""),
                new SqlParameter("@Keyword", filter.Keyword ?? ""),
                new SqlParameter("@IsHighlight", DbBoolFilter(filter.IsHighlight)),
                new SqlParameter("@IsExternal", DbBoolFilter(filter.IsExternal)),
                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize),
            };

            var items = new List<NewsResponseDto>();

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

        private NewsResponseDto Map(SqlDataReader r)
        {
            return new NewsResponseDto
            {
                NewsId = r.GetGuid(r.GetOrdinal("NewsId")),
                PosterId = r.GetGuid(r.GetOrdinal("PosterId")),
                ProjectId = r.IsDBNull(r.GetOrdinal("ProjectId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ProjectId")),
                ListingId = r.IsDBNull(r.GetOrdinal("ListingId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ListingId")),

                Title = r.GetString(r.GetOrdinal("Title")),
                Slug = r.IsDBNull(r.GetOrdinal("Slug")) ? "" : r.GetString(r.GetOrdinal("Slug")),
                ContentType = r.IsDBNull(r.GetOrdinal("ContentType")) ? "" : r.GetString(r.GetOrdinal("ContentType")),
                ShortDescription = r.IsDBNull(r.GetOrdinal("ShortDescription")) ? "" : r.GetString(r.GetOrdinal("ShortDescription")),
                Content = r.IsDBNull(r.GetOrdinal("Content")) ? "" : r.GetString(r.GetOrdinal("Content")),
                ThumbnailUrl = r.IsDBNull(r.GetOrdinal("ThumbnailUrl")) ? "" : r.GetString(r.GetOrdinal("ThumbnailUrl")),
                ImagesJson = r.IsDBNull(r.GetOrdinal("ImagesJson")) ? "" : r.GetString(r.GetOrdinal("ImagesJson")),
                Tags = r.IsDBNull(r.GetOrdinal("Tags")) ? "" : r.GetString(r.GetOrdinal("Tags")),

                PostedAt = r.GetDateTime(r.GetOrdinal("PostedAt")),
                Status = r.GetString(r.GetOrdinal("Status")),
                Views = r.GetInt32(r.GetOrdinal("Views")),
                IsHighlight = r.GetBoolean(r.GetOrdinal("IsHighlight")),
                IsExternal = r.GetBoolean(r.GetOrdinal("IsExternal")),

                CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt")),
                UpdatedAt = r.IsDBNull(r.GetOrdinal("UpdatedAt")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("UpdatedAt")),
            };
        }
    }
}
