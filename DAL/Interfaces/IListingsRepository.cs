using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_DAL.Interfaces
{
    public interface IListingsRepository
    {
        Task<Guid> CreateAsync(ListingsCreateRequestDto dto);
        Task<ListingsResponseDto> GetByIdAsync(Guid listingId);
        Task<bool> UpdateAsync(ListingsUpdateRequestDto dto);
        Task<bool> DeleteAsync(Guid listingId);
        Task<(List<ListingsResponseDto> Items, int Total)> GetListAsync(ListingsFilterDto filter);
    }
}
