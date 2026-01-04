using System;

namespace Common_DTOs.DTOs
{
    public class CommentsResponseDto
    {
        public Guid CommentId { get; set; }
        public Guid NewsId { get; set; }
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
        public Guid ParentId { get; set; }

        public string Content { get; set; } = string.Empty;
        public int LikesCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
