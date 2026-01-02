using System;

namespace Common_DTOs.DTOs
{
    public class NewsCreateRequestDto
    {
        public Guid PosterId { get; set; }
        public Guid ProjectId { get; set; }   // Guid.Empty => NULL
        public Guid ListingId { get; set; }   // Guid.Empty => NULL

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;          // "" => NULL (để DB cho phép null)
        public string ContentType { get; set; } = string.Empty;   // "" => NULL (DB default 'Article')
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string ImagesJson { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty; // "" => NULL (DB default 'Published')
        public int Views { get; set; } = -1;               // -1 => NULL (DB default 0)

        public int IsHighlight { get; set; } = -1; // -1 => NULL, 0/1 => bit
        public int IsExternal { get; set; } = -1;  // -1 => NULL, 0/1 => bit
    }
}
