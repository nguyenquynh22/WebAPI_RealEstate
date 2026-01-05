using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Property
{
    public Guid PropertyId { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? AreaId { get; set; }

    public string? UnitNumber { get; set; }

    public string? BlockOrTower { get; set; }

    public int? Floor { get; set; }

    public string? Status { get; set; }

    public Guid? OwnerId { get; set; }

    public decimal? OriginalAreaSqM { get; set; }

    public decimal? OriginalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ProjectArea? Area { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual User? Owner { get; set; }

    public virtual Project? Project { get; set; }
}
