using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Project
{
    public Guid ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? Developer { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<ProjectArea> ProjectAreas { get; set; } = new List<ProjectArea>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
