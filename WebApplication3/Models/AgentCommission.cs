using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class AgentCommission
{
    public Guid CommissionId { get; set; }

    public Guid AgentId { get; set; }

    public Guid ContractId { get; set; }

    public decimal BaseAmount { get; set; }

    public decimal CommissionPercent { get; set; }

    public decimal CommissionAmount { get; set; }

    public bool? IsPaid { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User Agent { get; set; } = null!;

    public virtual Contract Contract { get; set; } = null!;
}
