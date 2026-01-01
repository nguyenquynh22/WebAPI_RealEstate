using System;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_BLL.Interfaces
{
    public interface IPropertiesService
    {
        Task<Guid> CreateAsync(PropertiesCreateRequestDto dto);
        Task<PropertiesResponseDto> GetByIdAsync(Guid propertyId);
        Task<bool> UpdateAsync(PropertiesUpdateRequestDto dto);
        Task<bool> DeleteAsync(Guid propertyId);
        Task<object> GetListAsync(PropertiesFilterDto filter);
    }
}
