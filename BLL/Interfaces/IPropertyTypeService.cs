using Common_DTOs.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common_BLL.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<List<PropertyTypeResponseDto>> GetAllPropertyTypesAsync();
        Task<PropertyTypeResponseDto?> GetPropertyTypeByIdAsync(int id);
        Task<PropertyTypeResponseDto> CreatePropertyTypeAsync(PropertyTypeCreateRequestDto request);
        Task<PropertyTypeResponseDto?> UpdatePropertyTypeAsync(int id, PropertyTypeUpdateRequestDto request);
        Task<bool> DeletePropertyTypeAsync(int id);
    }
}