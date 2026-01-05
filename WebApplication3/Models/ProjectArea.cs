using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class ProjectArea
{
    public Guid AreaId { get; set; }

    public Guid ProjectId { get; set; }

    public string AreaName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<ProjectAreaPropertyType> ProjectAreaPropertyTypes { get; set; } = new List<ProjectAreaPropertyType>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
