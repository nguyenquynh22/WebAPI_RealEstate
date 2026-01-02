using System;

namespace Common_DTOs.DTOs
{
    public class NewsResponseDto
    {
        public Guid NewsId { get; set; }
        public Guid PosterId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ListingId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string ImagesJson { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;

        public DateTime PostedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Views { get; set; }

        public bool IsHighlight { get; set; }
        public bool IsExternal { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
