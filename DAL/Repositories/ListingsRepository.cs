using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_DAL.Repositories
{
    public class ListingsRepository : IListingsRepository
    {
        private readonly SqlHelper _sqlHelper;

        public ListingsRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        private object DbGuid(Guid v) => v == Guid.Empty ? DBNull.Value : v;
        private object DbString(string v) => string.IsNullOrWhiteSpace(v) ? DBNull.Value : v;

        // -1 => NULL (bỏ lọc), 0 => false, 1 => true
        private object DbBoolFilter(int v)
        {
            if (v == -1) return DBNull.Value;
            return v == 1;
        }

        public async Task<Guid> CreateAsync(ListingsCreateRequestDto dto)
        {
            string sql = @"
INSERT INTO dbo.Listings
(PropertyId, AgentId, ProjectId, AreaId, ProjectAreaPropertyTypeId,
 Title, Price, PriceUnit, DiscountAmount, AreaSqM, Bedrooms, Bathrooms,
 Address, Description, ImagesJson, VideosJson,
 Status, IsAvailable, IsFeatured, IsForRent, RentDurationUnit,
 PostedAt, Views, CreatedAt, UpdatedAt)
OUTPUT INSERTED.ListingId
VALUES
(@PropertyId, @AgentId, @ProjectId, @AreaId, @ProjectAreaPropertyTypeId,
 @Title, @Price, @PriceUnit, @DiscountAmount, @AreaSqM, @Bedrooms, @Bathrooms,
 @Address, @Description, @ImagesJson, @VideosJson,
 @Status, @IsAvailable, @IsFeatured, @IsForRent, @RentDurationUnit,
 GETDATE(), 0, GETDATE(), NULL);
";

            SqlParameter[] p =
            {
                new SqlParameter("@PropertyId", DbGuid(dto.PropertyId)),
                new SqlParameter("@AgentId", DbGuid(dto.AgentId)),
                new SqlParameter("@ProjectId", DbGuid(dto.ProjectId)),
                new SqlParameter("@AreaId", DbGuid(dto.AreaId)),
                new SqlParameter("@ProjectAreaPropertyTypeId", DbGuid(dto.ProjectAreaPropertyTypeId)),

                new SqlParameter("@Title", dto.Title),
                new SqlParameter("@Price", dto.Price),
                new SqlParameter("@PriceUnit", dto.PriceUnit),
                new SqlParameter("@DiscountAmount", dto.DiscountAmount),

                new SqlParameter("@AreaSqM", dto.AreaSqM == 0 ? DBNull.Value : dto.AreaSqM),
                new SqlParameter("@Bedrooms", dto.Bedrooms == 0 ? DBNull.Value : dto.Bedrooms),
                new SqlParameter("@Bathrooms", dto.Bathrooms == 0 ? DBNull.Value : dto.Bathrooms),

                new SqlParameter("@Address", DbString(dto.Address)),
                new SqlParameter("@Description", DbString(dto.Description)),

                new SqlParameter("@ImagesJson", DbString(dto.ImagesJson) ?? "[]"),
                new SqlParameter("@VideosJson", DbString(dto.VideosJson) ?? "[]"),

                new SqlParameter("@Status", dto.Status),
                new SqlParameter("@IsAvailable", dto.IsAvailable),
                new SqlParameter("@IsFeatured", dto.IsFeatured),
                new SqlParameter("@IsForRent", dto.IsForRent),
                new SqlParameter("@RentDurationUnit", DbString(dto.RentDurationUnit)),
            };

            object result = await _sqlHelper.ExecuteScalarAsync(sql, CommandType.Text, p);
            return (Guid)result;
        }

        public async Task<ListingsResponseDto> GetByIdAsync(Guid listingId)
        {
            string sql = @"
SELECT TOP 1
 ListingId, PropertyId, AgentId, ProjectId, AreaId, ProjectAreaPropertyTypeId,
 Title, Price, PriceUnit, DiscountAmount, AreaSqM, Bedrooms, Bathrooms,
 Address, Description, ImagesJson, VideosJson,
 Status, IsAvailable, IsFeatured, IsForRent, RentDurationUnit,
 PostedAt, Views, CreatedAt, UpdatedAt,
 IsSoldOrRented, ComputedPrice
FROM dbo.Listings
WHERE ListingId = @ListingId;
";
            SqlParameter[] p = { new SqlParameter("@ListingId", listingId) };

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text, p))
            {
                if (!await reader.ReadAsync()) return null;
                return Map(reader);
            }
        }

        public async Task<bool> UpdateAsync(ListingsUpdateRequestDto dto)
        {
            string sql = @"
UPDATE dbo.Listings
SET
 PropertyId = @PropertyId,
 AgentId = @AgentId,
 ProjectId = @ProjectId,
 AreaId = @AreaId,
 ProjectAreaPropertyTypeId = @ProjectAreaPropertyTypeId,

 Title = @Title,
 Price = @Price,
 PriceUnit = @PriceUnit,
 DiscountAmount = @DiscountAmount,
 AreaSqM = @AreaSqM,
 Bedrooms = @Bedrooms,
 Bathrooms = @Bathrooms,

 Address = @Address,
 Description = @Description,
 ImagesJson = @ImagesJson,
 VideosJson = @VideosJson,

 Status = @Status,
 IsAvailable = @IsAvailable,
 IsFeatured = @IsFeatured,
 IsForRent = @IsForRent,
 RentDurationUnit = @RentDurationUnit,

 UpdatedAt = GETDATE()
WHERE ListingId = @ListingId;
";

            SqlParameter[] p =
            {
                new SqlParameter("@ListingId", dto.ListingId),

                new SqlParameter("@PropertyId", DbGuid(dto.PropertyId)),
                new SqlParameter("@AgentId", DbGuid(dto.AgentId)),
                new SqlParameter("@ProjectId", DbGuid(dto.ProjectId)),
                new SqlParameter("@AreaId", DbGuid(dto.AreaId)),
                new SqlParameter("@ProjectAreaPropertyTypeId", DbGuid(dto.ProjectAreaPropertyTypeId)),

                new SqlParameter("@Title", dto.Title),
                new SqlParameter("@Price", dto.Price),
                new SqlParameter("@PriceUnit", dto.PriceUnit),
                new SqlParameter("@DiscountAmount", dto.DiscountAmount),

                new SqlParameter("@AreaSqM", dto.AreaSqM == 0 ? DBNull.Value : dto.AreaSqM),
                new SqlParameter("@Bedrooms", dto.Bedrooms == 0 ? DBNull.Value : dto.Bedrooms),
                new SqlParameter("@Bathrooms", dto.Bathrooms == 0 ? DBNull.Value : dto.Bathrooms),

                new SqlParameter("@Address", DbString(dto.Address)),
                new SqlParameter("@Description", DbString(dto.Description)),

                new SqlParameter("@ImagesJson", DbString(dto.ImagesJson) ?? "[]"),
                new SqlParameter("@VideosJson", DbString(dto.VideosJson) ?? "[]"),

                new SqlParameter("@Status", dto.Status),
                new SqlParameter("@IsAvailable", dto.IsAvailable),
                new SqlParameter("@IsFeatured", dto.IsFeatured),
                new SqlParameter("@IsForRent", dto.IsForRent),
                new SqlParameter("@RentDurationUnit", DbString(dto.RentDurationUnit)),
            };

            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, p);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid listingId)
        {
            string sql = @"DELETE FROM dbo.Listings WHERE ListingId = @ListingId;";
            SqlParameter[] p = { new SqlParameter("@ListingId", listingId) };

            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, p);
            return rows > 0;
        }

        public async Task<(List<ListingsResponseDto> Items, int Total)> GetListAsync(ListingsFilterDto filter)
        {
            int page = filter.Page <= 0 ? 1 : filter.Page;
            int pageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;
            int offset = (page - 1) * pageSize;

            string sqlItems = @"
SELECT
 ListingId, PropertyId, AgentId, ProjectId, AreaId, ProjectAreaPropertyTypeId,
 Title, Price, PriceUnit, DiscountAmount, AreaSqM, Bedrooms, Bathrooms,
 Address, Description, ImagesJson, VideosJson,
 Status, IsAvailable, IsFeatured, IsForRent, RentDurationUnit,
 PostedAt, Views, CreatedAt, UpdatedAt,
 IsSoldOrRented, ComputedPrice
FROM dbo.Listings
WHERE 1=1
 AND (@ProjectId IS NULL OR ProjectId = @ProjectId)
 AND (@AreaId IS NULL OR AreaId = @AreaId)
 AND (@AgentId IS NULL OR AgentId = @AgentId)
 AND (@PropertyId IS NULL OR PropertyId = @PropertyId)
 AND (@Status = '' OR Status = @Status)
 AND (@Keyword = '' OR Title LIKE '%' + @Keyword + '%' OR Address LIKE '%' + @Keyword + '%')
 AND (@IsForRent IS NULL OR IsForRent = @IsForRent)
 AND (@IsFeatured IS NULL OR IsFeatured = @IsFeatured)
ORDER BY PostedAt DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
";

            string sqlCount = @"
SELECT COUNT(1)
FROM dbo.Listings
WHERE 1=1
 AND (@ProjectId IS NULL OR ProjectId = @ProjectId)
 AND (@AreaId IS NULL OR AreaId = @AreaId)
 AND (@AgentId IS NULL OR AgentId = @AgentId)
 AND (@PropertyId IS NULL OR PropertyId = @PropertyId)
 AND (@Status = '' OR Status = @Status)
 AND (@Keyword = '' OR Title LIKE '%' + @Keyword + '%' OR Address LIKE '%' + @Keyword + '%')
 AND (@IsForRent IS NULL OR IsForRent = @IsForRent)
 AND (@IsFeatured IS NULL OR IsFeatured = @IsFeatured);
";

            SqlParameter[] p =
            {
                new SqlParameter("@ProjectId", DbGuid(filter.ProjectId)),
                new SqlParameter("@AreaId", DbGuid(filter.AreaId)),
                new SqlParameter("@AgentId", DbGuid(filter.AgentId)),
                new SqlParameter("@PropertyId", DbGuid(filter.PropertyId)),

                new SqlParameter("@Status", filter.Status ?? ""),
                new SqlParameter("@Keyword", filter.Keyword ?? ""),

                new SqlParameter("@IsForRent", DbBoolFilter(filter.IsForRent)),
                new SqlParameter("@IsFeatured", DbBoolFilter(filter.IsFeatured)),

                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize),
            };

            List<ListingsResponseDto> items = new List<ListingsResponseDto>();

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sqlItems, CommandType.Text, p))
            {
                while (await reader.ReadAsync())
                {
                    items.Add(Map(reader));
                }
            }

            object totalObj = await _sqlHelper.ExecuteScalarAsync(sqlCount, CommandType.Text, p);
            int total = Convert.ToInt32(totalObj);

            return (items, total);
        }

        private ListingsResponseDto Map(SqlDataReader r)
        {
            var dto = new ListingsResponseDto();

            dto.ListingId = r.GetGuid(r.GetOrdinal("ListingId"));

            dto.PropertyId = r.IsDBNull(r.GetOrdinal("PropertyId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("PropertyId"));
            dto.AgentId = r.IsDBNull(r.GetOrdinal("AgentId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("AgentId"));
            dto.ProjectId = r.IsDBNull(r.GetOrdinal("ProjectId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ProjectId"));
            dto.AreaId = r.IsDBNull(r.GetOrdinal("AreaId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("AreaId"));
            dto.ProjectAreaPropertyTypeId = r.IsDBNull(r.GetOrdinal("ProjectAreaPropertyTypeId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ProjectAreaPropertyTypeId"));

            dto.Title = r.IsDBNull(r.GetOrdinal("Title")) ? "" : r.GetString(r.GetOrdinal("Title"));

            dto.Price = r.IsDBNull(r.GetOrdinal("Price")) ? 0 : r.GetDecimal(r.GetOrdinal("Price"));
            dto.PriceUnit = r.IsDBNull(r.GetOrdinal("PriceUnit")) ? "" : r.GetString(r.GetOrdinal("PriceUnit"));
            dto.DiscountAmount = r.IsDBNull(r.GetOrdinal("DiscountAmount")) ? 0 : r.GetDecimal(r.GetOrdinal("DiscountAmount"));

            dto.AreaSqM = r.IsDBNull(r.GetOrdinal("AreaSqM")) ? 0 : r.GetDecimal(r.GetOrdinal("AreaSqM"));
            dto.Bedrooms = r.IsDBNull(r.GetOrdinal("Bedrooms")) ? 0 : r.GetInt32(r.GetOrdinal("Bedrooms"));
            dto.Bathrooms = r.IsDBNull(r.GetOrdinal("Bathrooms")) ? 0 : r.GetInt32(r.GetOrdinal("Bathrooms"));

            dto.Address = r.IsDBNull(r.GetOrdinal("Address")) ? "" : r.GetString(r.GetOrdinal("Address"));
            dto.Description = r.IsDBNull(r.GetOrdinal("Description")) ? "" : r.GetString(r.GetOrdinal("Description"));

            dto.ImagesJson = r.IsDBNull(r.GetOrdinal("ImagesJson")) ? "[]" : r.GetString(r.GetOrdinal("ImagesJson"));
            dto.VideosJson = r.IsDBNull(r.GetOrdinal("VideosJson")) ? "[]" : r.GetString(r.GetOrdinal("VideosJson"));

            dto.Status = r.IsDBNull(r.GetOrdinal("Status")) ? "" : r.GetString(r.GetOrdinal("Status"));

            dto.IsAvailable = !r.IsDBNull(r.GetOrdinal("IsAvailable")) && r.GetBoolean(r.GetOrdinal("IsAvailable"));
            dto.IsFeatured = !r.IsDBNull(r.GetOrdinal("IsFeatured")) && r.GetBoolean(r.GetOrdinal("IsFeatured"));
            dto.IsForRent = !r.IsDBNull(r.GetOrdinal("IsForRent")) && r.GetBoolean(r.GetOrdinal("IsForRent"));

            dto.RentDurationUnit = r.IsDBNull(r.GetOrdinal("RentDurationUnit")) ? "" : r.GetString(r.GetOrdinal("RentDurationUnit"));

            dto.PostedAt = r.GetDateTime(r.GetOrdinal("PostedAt"));
            dto.Views = r.IsDBNull(r.GetOrdinal("Views")) ? 0 : r.GetInt32(r.GetOrdinal("Views"));

            dto.CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt"));
            dto.UpdatedAt = r.IsDBNull(r.GetOrdinal("UpdatedAt")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("UpdatedAt"));

            dto.IsSoldOrRented = !r.IsDBNull(r.GetOrdinal("IsSoldOrRented")) && r.GetBoolean(r.GetOrdinal("IsSoldOrRented"));
            dto.ComputedPrice = r.IsDBNull(r.GetOrdinal("ComputedPrice")) ? dto.Price : r.GetDecimal(r.GetOrdinal("ComputedPrice"));

            return dto;
        }
    }
}
