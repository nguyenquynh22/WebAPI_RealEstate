using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_BLL.Interfaces
{
    public interface ICommentsService
    {
        Task<Guid> CreateAsync(CommentsCreateRequestDto dto);
        Task<CommentsResponseDto> GetByIdAsync(Guid commentId);
        Task<bool> UpdateAsync(CommentsUpdateRequestDto dto);
        Task<bool> DeleteAsync(Guid commentId);
        Task<(List<CommentsResponseDto> Items, int Total)> GetListAsync(CommentsFilterDto filter);
    }
}
