using System.Collections.Generic;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_BLL.Services
{
    public class ConversationsService : IConversationsService
    {
        private readonly IConversationsRepository _repo;

        public ConversationsService(IConversationsRepository repo)
        {
            _repo = repo;
        }

        public Task<string> CreateOrGetAsync(ConversationsCreateRequestDto dto) => _repo.CreateOrGetAsync(dto);
        public Task<ConversationsResponseDto> GetByIdAsync(string conversationId) => _repo.GetByIdAsync(conversationId);
        public Task<(List<ConversationsResponseDto> Items, int Total)> GetListAsync(ConversationsFilterDto filter) => _repo.GetListAsync(filter);
        public Task<bool> DeleteAsync(string conversationId) => _repo.DeleteAsync(conversationId);
    }
}
