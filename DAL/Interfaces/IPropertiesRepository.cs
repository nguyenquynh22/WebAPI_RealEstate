using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_DAL.Interfaces
{
    public interface IPropertiesRepository
    {
        Task<Guid> CreateAsync(PropertiesCreateRequestDto dto);
        Task<PropertiesResponseDto> GetByIdAsync(Guid propertyId);
        Task<bool> UpdateAsync(PropertiesUpdateRequestDto dto);
        Task<bool> DeleteAsync(Guid propertyId);
        Task<(List<PropertiesResponseDto> Items, int Total)> GetListAsync(PropertiesFilterDto filter);
    }
}
