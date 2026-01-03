using System;

namespace Common_DTOs.DTOs
{
    public class CommentsUpdateRequestDto
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
