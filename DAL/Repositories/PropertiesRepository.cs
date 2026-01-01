using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_DAL.Repositories
{
    public class PropertiesRepository : IPropertiesRepository
    {
        private readonly SqlHelper _sqlHelper;

        public PropertiesRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        // ===== helper convert =====
        private object DbGuid(Guid value) => value == Guid.Empty ? DBNull.Value : value;
        private object DbString(string value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;
        private object DbInt(int value) => value == 0 ? DBNull.Value : value;

        public async Task<Guid> CreateAsync(PropertiesCreateRequestDto dto)
        {
            string sql = @"
INSERT INTO dbo.Properties
(ProjectId, AreaId, UnitNumber, BlockOrTower, Floor, Status, OwnerId, OriginalAreaSqM, OriginalPrice, CreatedAt, UpdatedAt)
OUTPUT INSERTED.PropertyId
VALUES
(@ProjectId, @AreaId, @UnitNumber, @BlockOrTower, @Floor, @Status, @OwnerId, @OriginalAreaSqM, @OriginalPrice, GETDATE(), NULL);
";

            SqlParameter[] p =
            {
                new SqlParameter("@ProjectId", DbGuid(dto.ProjectId)),
                new SqlParameter("@AreaId", DbGuid(dto.AreaId)),
                new SqlParameter("@UnitNumber", DbString(dto.UnitNumber)),
                new SqlParameter("@BlockOrTower", DbString(dto.BlockOrTower)),
                new SqlParameter("@Floor", DbInt(dto.Floor)),
                new SqlParameter("@Status", dto.Status),
                new SqlParameter("@OwnerId", DbGuid(dto.OwnerId)),
                new SqlParameter("@OriginalAreaSqM", dto.OriginalAreaSqM),
                new SqlParameter("@OriginalPrice", dto.OriginalPrice),
            };

            object result = await _sqlHelper.ExecuteScalarAsync(sql, CommandType.Text, p);
            return (Guid)result;
        }

        public async Task<PropertiesResponseDto> GetByIdAsync(Guid propertyId)
        {
            string sql = @"
SELECT TOP 1
  PropertyId, ProjectId, AreaId, UnitNumber, BlockOrTower, Floor, Status, OwnerId,
  OriginalAreaSqM, OriginalPrice, CreatedAt, UpdatedAt
FROM dbo.Properties
WHERE PropertyId = @PropertyId;
";

            SqlParameter[] p = { new SqlParameter("@PropertyId", propertyId) };

            using (var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text, p))
            {
                if (!await reader.ReadAsync()) return null;
                return Map(reader);
            }
        }

        public async Task<bool> UpdateAsync(PropertiesUpdateRequestDto dto)
        {
            string sql = @"
UPDATE dbo.Properties
SET
  ProjectId = @ProjectId,
  AreaId = @AreaId,
  UnitNumber = @UnitNumber,
  BlockOrTower = @BlockOrTower,
  Floor = @Floor,
  Status = @Status,
  OwnerId = @OwnerId,
  OriginalAreaSqM = @OriginalAreaSqM,
  OriginalPrice = @OriginalPrice,
  UpdatedAt = GETDATE()
WHERE PropertyId = @PropertyId;
";

            SqlParameter[] p =
            {
                new SqlParameter("@PropertyId", dto.PropertyId),
                new SqlParameter("@ProjectId", DbGuid(dto.ProjectId)),
                new SqlParameter("@AreaId", DbGuid(dto.AreaId)),
                new SqlParameter("@UnitNumber", DbString(dto.UnitNumber)),
                new SqlParameter("@BlockOrTower", DbString(dto.BlockOrTower)),
                new SqlParameter("@Floor", DbInt(dto.Floor)),
                new SqlParameter("@Status", dto.Status),
                new SqlParameter("@OwnerId", DbGuid(dto.OwnerId)),
                new SqlParameter("@OriginalAreaSqM", dto.OriginalAreaSqM),
                new SqlParameter("@OriginalPrice", dto.OriginalPrice),
            };

            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, p);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid propertyId)
        {
            string sql = @"DELETE FROM dbo.Properties WHERE PropertyId = @PropertyId;";
            SqlParameter[] p = { new SqlParameter("@PropertyId", propertyId) };

            int rows = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, p);
            return rows > 0;
        }

        public async Task<(List<PropertiesResponseDto> Items, int Total)> GetListAsync(PropertiesFilterDto filter)
        {
            int page = filter.Page <= 0 ? 1 : filter.Page;
            int pageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;
            int offset = (page - 1) * pageSize;

            // 1) items
            string sqlItems = @"
SELECT
  PropertyId, ProjectId, AreaId, UnitNumber, BlockOrTower, Floor, Status, OwnerId,
  OriginalAreaSqM, OriginalPrice, CreatedAt, UpdatedAt
FROM dbo.Properties
WHERE 1=1
  AND (@ProjectId IS NULL OR ProjectId = @ProjectId)
  AND (@AreaId IS NULL OR AreaId = @AreaId)
  AND (@OwnerId IS NULL OR OwnerId = @OwnerId)
  AND (@Status = '' OR Status = @Status)
  AND (@UnitNumber = '' OR UnitNumber LIKE '%' + @UnitNumber + '%')
ORDER BY CreatedAt DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
";

            // 2) total
            string sqlCount = @"
SELECT COUNT(1)
FROM dbo.Properties
WHERE 1=1
  AND (@ProjectId IS NULL OR ProjectId = @ProjectId)
  AND (@AreaId IS NULL OR AreaId = @AreaId)
  AND (@OwnerId IS NULL OR OwnerId = @OwnerId)
  AND (@Status = '' OR Status = @Status)
  AND (@UnitNumber = '' OR UnitNumber LIKE '%' + @UnitNumber + '%');
";

            // ✅ Đây là chỗ bạn bị lỗi: filter.Status là string nên dùng ?? "" OK
            SqlParameter[] p =
            {
                new SqlParameter("@ProjectId", DbGuid(filter.ProjectId)),
                new SqlParameter("@AreaId", DbGuid(filter.AreaId)),
                new SqlParameter("@OwnerId", DbGuid(filter.OwnerId)),
                new SqlParameter("@Status", filter.Status ?? ""),
                new SqlParameter("@UnitNumber", filter.UnitNumber ?? ""),
                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize),
            };

            List<PropertiesResponseDto> items = new List<PropertiesResponseDto>();

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

        private PropertiesResponseDto Map(SqlDataReader r)
        {
            PropertiesResponseDto dto = new PropertiesResponseDto();

            dto.PropertyId = r.GetGuid(r.GetOrdinal("PropertyId"));

            dto.ProjectId = r.IsDBNull(r.GetOrdinal("ProjectId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("ProjectId"));
            dto.AreaId = r.IsDBNull(r.GetOrdinal("AreaId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("AreaId"));

            dto.UnitNumber = r.IsDBNull(r.GetOrdinal("UnitNumber")) ? string.Empty : r.GetString(r.GetOrdinal("UnitNumber"));
            dto.BlockOrTower = r.IsDBNull(r.GetOrdinal("BlockOrTower")) ? string.Empty : r.GetString(r.GetOrdinal("BlockOrTower"));

            dto.Floor = r.IsDBNull(r.GetOrdinal("Floor")) ? 0 : r.GetInt32(r.GetOrdinal("Floor"));
            dto.Status = r.IsDBNull(r.GetOrdinal("Status")) ? string.Empty : r.GetString(r.GetOrdinal("Status"));

            dto.OwnerId = r.IsDBNull(r.GetOrdinal("OwnerId")) ? Guid.Empty : r.GetGuid(r.GetOrdinal("OwnerId"));

            dto.OriginalAreaSqM = r.GetDecimal(r.GetOrdinal("OriginalAreaSqM"));
            dto.OriginalPrice = r.GetDecimal(r.GetOrdinal("OriginalPrice"));

            dto.CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt"));
            dto.UpdatedAt = r.IsDBNull(r.GetOrdinal("UpdatedAt")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("UpdatedAt"));

            return dto;
        }
    }
}
