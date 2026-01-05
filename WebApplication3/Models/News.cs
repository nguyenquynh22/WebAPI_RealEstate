using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class News
{
    public Guid NewsId { get; set; }

    public Guid PosterId { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? ListingId { get; set; }

    public string Title { get; set; } = null!;

    public string? Slug { get; set; }

    public string? ContentType { get; set; }

    public string? ShortDescription { get; set; }

    public string? Content { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? ImagesJson { get; set; }

    public string? Tags { get; set; }

    public DateTime? PostedAt { get; set; }

    public string? Status { get; set; }

    public int? Views { get; set; }

    public bool? IsHighlight { get; set; }

    public bool? IsExternal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Listing? Listing { get; set; }

    public virtual User Poster { get; set; } = null!;

    public virtual Project? Project { get; set; }
}
