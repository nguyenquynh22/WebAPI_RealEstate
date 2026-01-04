using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.DTOs;

namespace Common_BLL.Interfaces
{
    public interface IConversationsService
    {
        Task<string> CreateOrGetAsync(ConversationsCreateRequestDto dto);
        Task<ConversationsResponseDto> GetByIdAsync(string conversationId);
        Task<(List<ConversationsResponseDto> Items, int Total)> GetListAsync(ConversationsFilterDto filter);
        Task<bool> DeleteAsync(string conversationId);
    }
}
