using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Contract
{
    public Guid ContractId { get; set; }

    public Guid ListingId { get; set; }

    public Guid SellerId { get; set; }

    public Guid BuyerId { get; set; }

    public string ContractType { get; set; } = null!;

    public decimal Price { get; set; }

    public string PriceUnit { get; set; } = null!;

    public int? DurationMonths { get; set; }

    public decimal? DepositAmount { get; set; }

    public decimal? DownPayment { get; set; }

    public string? Status { get; set; }

    public string? PdfDocumentUrl { get; set; }

    public DateTime? SigningDate { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual AgentCommission? AgentCommission { get; set; }

    public virtual User Buyer { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Listing Listing { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
