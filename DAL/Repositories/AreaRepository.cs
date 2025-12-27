using Common_DAL.Interfaces;
using Common_DTOs.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common_DAL.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly SqlHelper _sqlHelper;
        public AreaRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper; 
        }
        public ProjectAreas MapToArea(System.Data.Common.DbDataReader reader)
        {
            return new ProjectAreas
            {
                AreaId = reader.GetGuid(reader.GetOrdinal("AreaId")),
                AreaName = reader.GetString(reader.GetOrdinal("AreaName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }
        public async Task<List<ProjectAreas>> GetAllAreasAsync()
        {
            var areas = new List<ProjectAreas>();
            string sql = "SELECT * FROM ProjectAreas";
            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, System.Data.CommandType.Text);
            while (await reader.ReadAsync())
            {
                areas.Add(MapToArea(reader));
            }
            return areas;
        }
        public async Task<ProjectAreas?> GetAreaByIdAsync(Guid areaId)
        {
            string sql = "SELECT TOP 1 * FROM ProjectAreas WHERE AreaId = @AreaId";
            var parameter = new Microsoft.Data.SqlClient.SqlParameter("@AreaId", areaId);
            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, System.Data.CommandType.Text, parameter);
            if (await reader.ReadAsync())
            {
                return MapToArea(reader);
            }
            return null;
        }
        public async Task<ProjectAreas> CreateAreaAsync(ProjectAreas area)
        {
            string sql = @"
                INSERT INTO ProjectAreas (AreaId, AreaName, Description, CreatedAt)
                VALUES (@AreaId, @AreaName, @Description, @CreatedAt)";
            var parameters = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@AreaId", area.AreaId),
                new Microsoft.Data.SqlClient.SqlParameter("@AreaName", area.AreaName),
                new Microsoft.Data.SqlClient.SqlParameter("@Description", (object?)area.Description ?? DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@CreatedAt", area.CreatedAt)
            };
            await _sqlHelper.ExecuteNonQueryAsync(sql, System.Data.CommandType.Text, parameters);
            return area;
        }
        public async Task<ProjectAreas> UpdateAreaAsync(ProjectAreas area)
        {
            area.UpdatedAt = DateTime.Now;

            string sql = @"UPDATE ProjectAreas 
                   SET AreaName = @AreaName, Description = @Description, UpdatedAt = @UpdatedAt 
                   WHERE AreaId = @AreaId";

            var parameters = new[] {
                new SqlParameter("@AreaId", area.AreaId),
                new SqlParameter("@AreaName", area.AreaName),
                new SqlParameter("@Description", (object?)area.Description ?? DBNull.Value),
                new SqlParameter("@UpdatedAt", (object?)area.UpdatedAt ?? DBNull.Value)
            };

            await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return area;
        }
        public async Task<bool> DeleteAreaAsync(Guid areaId)
        {
            string sql = "DELETE FROM ProjectAreas WHERE AreaId = @AreaId";
            var parameter = new Microsoft.Data.SqlClient.SqlParameter("@AreaId", areaId);
            int rowsAffected = await _sqlHelper.ExecuteNonQueryAsync(sql, System.Data.CommandType.Text, parameter);
            return rowsAffected > 0;
        }
    }
}
