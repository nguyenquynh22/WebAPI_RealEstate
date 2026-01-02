using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_DAL.Interfaces
{
    public interface INewsRepository
    {
        Task<Guid> CreateAsync(NewsCreateRequestDto dto);
        Task<NewsResponseDto> GetByIdAsync(Guid newsId);
        Task<bool> UpdateAsync(NewsUpdateRequestDto dto);
        Task<bool> DeleteAsync(Guid newsId);
        Task<(List<NewsResponseDto> Items, int Total)> GetListAsync(NewsFilterDto filter);
    }
}
