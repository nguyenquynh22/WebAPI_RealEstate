using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_DAL.Interfaces
{
    public interface ICommentsRepository
    {
        Task<Guid> CreateAsync(CommentsCreateRequestDto dto);
        Task<CommentsResponseDto> GetByIdAsync(Guid commentId);
        Task<bool> UpdateAsync(CommentsUpdateRequestDto dto);
        Task<bool> DeleteAsync(Guid commentId);
        Task<(List<CommentsResponseDto> Items, int Total)> GetListAsync(CommentsFilterDto filter);
    }
}
