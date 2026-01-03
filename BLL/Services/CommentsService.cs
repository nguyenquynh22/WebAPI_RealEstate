using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_BLL.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _repo;

        public CommentsService(ICommentsRepository repo)
        {
            _repo = repo;
        }

        public Task<Guid> CreateAsync(CommentsCreateRequestDto dto) => _repo.CreateAsync(dto);
        public Task<CommentsResponseDto> GetByIdAsync(Guid commentId) => _repo.GetByIdAsync(commentId);
        public Task<bool> UpdateAsync(CommentsUpdateRequestDto dto) => _repo.UpdateAsync(dto);
        public Task<bool> DeleteAsync(Guid commentId) => _repo.DeleteAsync(commentId);
        public Task<(List<CommentsResponseDto> Items, int Total)> GetListAsync(CommentsFilterDto filter)
            => _repo.GetListAsync(filter);
    }
}
