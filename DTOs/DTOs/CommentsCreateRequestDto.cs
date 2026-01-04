using System;

namespace Common_DTOs.DTOs
{
    public class CommentsCreateRequestDto
    {
        public Guid NewsId { get; set; } = Guid.Empty;     // Guid.Empty => NULL
        public Guid ListingId { get; set; } = Guid.Empty;  // Guid.Empty => NULL
        public Guid UserId { get; set; }                   // NOT NULL
        public Guid ParentId { get; set; } = Guid.Empty;   // Guid.Empty => NULL
        public string Content { get; set; } = string.Empty; // NOT NULL
    }
}
