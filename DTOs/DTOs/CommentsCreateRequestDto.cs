using System;

namespace Common_DTOs.DTOs
{
    public class CommentsCreateRequestDto
    {
        public Guid UserId { get; set; }        // người comment
        public Guid NewsId { get; set; }        // Guid.Empty => NULL
        public Guid ListingId { get; set; }     // Guid.Empty => NULL
        public Guid ParentId { get; set; }      // Guid.Empty => NULL (reply)
        public string Content { get; set; } = string.Empty;
    }
}
