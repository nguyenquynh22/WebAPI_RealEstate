using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class CommissionRate
{
    public int RateId { get; set; }

    public string ForType { get; set; } = null!;

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public decimal RatePercentage { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
