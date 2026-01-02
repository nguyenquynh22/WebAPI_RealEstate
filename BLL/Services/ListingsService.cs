using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_BLL.Services
{
    public class ListingsService : IListingsService
    {
        private readonly IListingsRepository _repo;

        public ListingsService(IListingsRepository repo)
        {
            _repo = repo;
        }

        public Task<Guid> CreateAsync(ListingsCreateRequestDto dto) => _repo.CreateAsync(dto);
        public Task<ListingsResponseDto> GetByIdAsync(Guid listingId) => _repo.GetByIdAsync(listingId);
        public Task<bool> UpdateAsync(ListingsUpdateRequestDto dto) => _repo.UpdateAsync(dto);
        public Task<bool> DeleteAsync(Guid listingId) => _repo.DeleteAsync(listingId);
        public Task<(List<ListingsResponseDto> Items, int Total)> GetListAsync(ListingsFilterDto filter) => _repo.GetListAsync(filter);
    }
}
