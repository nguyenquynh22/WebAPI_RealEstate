using Common_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_DAL.Interfaces;
using Common_DTOs.Entities;
using Microsoft.Data.SqlClient;

namespace Common_DAL.Repositories
{
    public class PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly SqlHelper _sqlHelper;

        public PropertyTypeRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<List<PropertyTypes>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = new List<PropertyTypes>();
            string sql = "SELECT * FROM PropertyTypes ORDER BY TypeName ASC";
            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, System.Data.CommandType.Text);
            while (await reader.ReadAsync())
            {
                propertyTypes.Add(MapToPropertyType(reader));
            }
            return propertyTypes;
        }

        public async Task<PropertyTypes?> GetPropertyTypeByIdAsync(int propertyTypeId)
        {
            string sql = "SELECT TOP 1 * FROM PropertyTypes WHERE PropertyTypeId = @Id";
            var parameter = new SqlParameter("@Id", propertyTypeId);
            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, System.Data.CommandType.Text, parameter);
            if (await reader.ReadAsync())
            {
                return MapToPropertyType(reader);
            }
            return null;
        }
        public async Task<int> CreatePropertyTypeAsync(PropertyTypes propertyType)
        {
            string sql = @"INSERT INTO PropertyTypes (TypeName, CreatedAt) 
                       VALUES (@TypeName, GETDATE());
                       SELECT CAST(SCOPE_IDENTITY() as int);";
            var parameter = new SqlParameter("@TypeName", propertyType.TypeName);
            var result = await _sqlHelper.ExecuteScalarAsync(sql, System.Data.CommandType.Text, parameter);
            return (int)result;
        }

        public async Task<bool> UpdatePropertyTypeAsync(PropertyTypes propertyType)
        {
            string sql = @"UPDATE PropertyTypes SET TypeName = @TypeName, UpdatedAt = GETDATE() WHERE PropertyTypeId = @Id";
            var parameters = new[]
            {
            new SqlParameter("@Id", propertyType.PropertyTypeId),
            new SqlParameter("@TypeName", propertyType.TypeName)
        };
            int result = await _sqlHelper.ExecuteNonQueryAsync(sql, System.Data.CommandType.Text, parameters);
            return result > 0;
        }

        public async Task<bool> DeletePropertyTypeAsync(int propertyTypeId)
        {
            string sql = "DELETE FROM PropertyTypes WHERE PropertyTypeId = @Id";
            var parameter = new SqlParameter("@Id", propertyTypeId);
            int result = await _sqlHelper.ExecuteNonQueryAsync(sql, System.Data.CommandType.Text, parameter);
            return result > 0;
        }
        private PropertyTypes MapToPropertyType(SqlDataReader reader)
        {
            return new PropertyTypes
            {
                PropertyTypeId = (int)reader["PropertyTypeId"],
                TypeName = reader["TypeName"]?.ToString() ?? string.Empty,
                CreatedAt = reader["CreatedAt"] as DateTime?,
                UpdatedAt = reader["UpdatedAt"] as DateTime? 
            };
        }
    }
}
