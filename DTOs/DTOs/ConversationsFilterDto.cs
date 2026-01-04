using System;

namespace Common_DTOs.DTOs
{
    public class ConversationsFilterDto
    {
        public Guid UserId { get; set; } = Guid.Empty; // lọc theo user
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
