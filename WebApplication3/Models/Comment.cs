using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Comment
{
    public Guid CommentId { get; set; }

    public Guid? NewsId { get; set; }

    public Guid? ListingId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public int? LikesCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    public virtual Listing? Listing { get; set; }

    public virtual News? News { get; set; }

    public virtual Comment? Parent { get; set; }

    public virtual User User { get; set; } = null!;
}
