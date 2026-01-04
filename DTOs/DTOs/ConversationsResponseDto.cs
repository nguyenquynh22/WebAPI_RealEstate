using System;

namespace Common_DTOs.DTOs
{
    public class ConversationsResponseDto
    {
        public string ConversationId { get; set; } = string.Empty;
        public Guid Participant1Id { get; set; }
        public Guid Participant2Id { get; set; }

        public DateTime LastMessageAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
