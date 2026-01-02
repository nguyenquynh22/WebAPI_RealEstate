using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _repo;

        public NewsService(INewsRepository repo)
        {
            _repo = repo;
        }

        public Task<Guid> CreateAsync(NewsCreateRequestDto dto) => _repo.CreateAsync(dto);
        public Task<NewsResponseDto> GetByIdAsync(Guid newsId) => _repo.GetByIdAsync(newsId);
        public Task<bool> UpdateAsync(NewsUpdateRequestDto dto) => _repo.UpdateAsync(dto);
        public Task<bool> DeleteAsync(Guid newsId) => _repo.DeleteAsync(newsId);
        public Task<(List<NewsResponseDto> Items, int Total)> GetListAsync(NewsFilterDto filter) => _repo.GetListAsync(filter);
    }
}
