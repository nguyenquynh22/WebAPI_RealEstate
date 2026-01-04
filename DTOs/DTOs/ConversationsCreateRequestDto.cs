using System;

namespace Common_DTOs.DTOs
{
    public class ConversationsCreateRequestDto
    {
        public Guid Participant1Id { get; set; }
        public Guid Participant2Id { get; set; }
    }
}
