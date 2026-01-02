using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;

namespace Common_BLL.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _repo;

        public PropertyTypeService(IPropertyTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PropertyTypeResponseDto>> GetAllPropertyTypesAsync()
        {
            var entities = await _repo.GetAllPropertyTypesAsync();
            return entities.Select(e => MapToDto(e)).ToList();
        }

        public async Task<PropertyTypeResponseDto?> GetPropertyTypeByIdAsync(int id)
        {
            var entity = await _repo.GetPropertyTypeByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<PropertyTypeResponseDto> CreatePropertyTypeAsync(PropertyTypeCreateRequestDto request)
        {
            var entity = new PropertyTypes { TypeName = request.TypeName };
            int newId = await _repo.CreatePropertyTypeAsync(entity);
            var created = await _repo.GetPropertyTypeByIdAsync(newId);
            return MapToDto(created!);
        }

        public async Task<PropertyTypeResponseDto?> UpdatePropertyTypeAsync(int id, PropertyTypeUpdateRequestDto request)
        {
            var existing = await _repo.GetPropertyTypeByIdAsync(id);
            if (existing == null) return null;
            existing.TypeName = request.TypeName;
            bool isSuccess = await _repo.UpdatePropertyTypeAsync(existing);
            if (isSuccess)
            {
                var updated = await _repo.GetPropertyTypeByIdAsync(id);
                return MapToDto(updated!);
            }
            return null;
        }

        public async Task<bool> DeletePropertyTypeAsync(int id)
        {
            return await _repo.DeletePropertyTypeAsync(id);
        }

        private PropertyTypeResponseDto MapToDto(PropertyTypes entity)
        {
            return new PropertyTypeResponseDto
            {
                PropertyTypeId = entity.PropertyTypeId,
                TypeName = entity.TypeName
            };
        }
    }
}