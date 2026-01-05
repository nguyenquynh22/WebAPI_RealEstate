using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Listing
{
    public Guid ListingId { get; set; }

    public Guid? PropertyId { get; set; }

    public Guid? AgentId { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? AreaId { get; set; }

    public Guid? ProjectAreaPropertyTypeId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public string PriceUnit { get; set; } = null!;

    public decimal? DiscountAmount { get; set; }

    public decimal? AreaSqM { get; set; }

    public int? Bedrooms { get; set; }

    public int? Bathrooms { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public string? ImagesJson { get; set; }

    public string? VideosJson { get; set; }

    public string? Status { get; set; }

    public bool? IsAvailable { get; set; }

    public bool? IsFeatured { get; set; }

    public bool? IsForRent { get; set; }

    public string? RentDurationUnit { get; set; }

    public DateTime? PostedAt { get; set; }

    public int? Views { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int IsSoldOrRented { get; set; }

    public decimal? ComputedPrice { get; set; }

    public virtual User? Agent { get; set; }

    public virtual ProjectArea? Area { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual Project? Project { get; set; }

    public virtual ProjectAreaPropertyType? ProjectAreaPropertyType { get; set; }

    public virtual Property? Property { get; set; }
}
