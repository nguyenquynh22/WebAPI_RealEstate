using System;

namespace Common_DTOs.DTOs
{
    public class NewsUpdateRequestDto
    {
        public Guid NewsId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string ImagesJson { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty; // Draft/Published/Archived
        public int IsHighlight { get; set; } = -1;         // -1 ignore
        public int IsExternal { get; set; } = -1;          // -1 ignore
    }
}
