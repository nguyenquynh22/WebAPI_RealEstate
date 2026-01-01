using System;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_BLL.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly IPropertiesRepository _repo;

        public PropertiesService(IPropertiesRepository repo)
        {
            _repo = repo;
        }

        public Task<Guid> CreateAsync(PropertiesCreateRequestDto dto)
        {
            if (dto.UnitNumber == null) dto.UnitNumber = string.Empty;
            if (dto.BlockOrTower == null) dto.BlockOrTower = string.Empty;
            if (string.IsNullOrWhiteSpace(dto.Status)) dto.Status = "Available";
            return _repo.CreateAsync(dto);
        }

        public Task<PropertiesResponseDto> GetByIdAsync(Guid propertyId)
        {
            return _repo.GetByIdAsync(propertyId);
        }

        public Task<bool> UpdateAsync(PropertiesUpdateRequestDto dto)
        {
            if (dto.UnitNumber == null) dto.UnitNumber = string.Empty;
            if (dto.BlockOrTower == null) dto.BlockOrTower = string.Empty;
            if (string.IsNullOrWhiteSpace(dto.Status)) dto.Status = "Available";
            return _repo.UpdateAsync(dto);
        }

        public Task<bool> DeleteAsync(Guid propertyId)
        {
            return _repo.DeleteAsync(propertyId);
        }

        public async Task<object> GetListAsync(PropertiesFilterDto filter)
        {
            var (items, total) = await _repo.GetListAsync(filter);
            return new
            {
                items = items,
                total = total,
                page = filter.Page,
                pageSize = filter.PageSize
            };
        }
    }
}
