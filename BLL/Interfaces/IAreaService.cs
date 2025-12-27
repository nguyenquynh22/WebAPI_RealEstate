using Common_DTOs.DTOs;
using Common_DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_BLL.Interfaces
{
    public interface IAreaService
    {
        Task<List<AreaResponseDto>> GetAllAreasAsync(); 
        Task<AreaResponseDto?> GetAreaByIdAsync(Guid areaId);
        Task<AreaResponseDto> CreateAreaAsync(AreaCreateRequestDto request);
        Task<AreaResponseDto> UpdateAreaAsync(AreaUpdateRequestDto request);
        Task<bool> DeleteAreaAsync(Guid areaId);
    }
}
