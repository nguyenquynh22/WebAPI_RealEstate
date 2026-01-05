using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class ProjectAreaPropertyType
{
    public Guid ProjectAreaPropertyTypeId { get; set; }

    public Guid AreaId { get; set; }

    public int PropertyTypeId { get; set; }

    public virtual ProjectArea Area { get; set; } = null!;

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual PropertyType PropertyType { get; set; } = null!;
}
