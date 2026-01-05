using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Invoice
{
    public Guid InvoiceId { get; set; }

    public Guid? ContractId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Currency { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? InvoicePdfUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
