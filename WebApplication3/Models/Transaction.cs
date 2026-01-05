using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid? ContractId { get; set; }

    public Guid? InvoiceId { get; set; }

    public decimal Amount { get; set; }

    public string? Currency { get; set; }

    public string? Purpose { get; set; }

    public DateTime? TransactionDate { get; set; }

    public Guid PayerId { get; set; }

    public Guid ReceiverId { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public bool? IsPartial { get; set; }

    public string? Reference { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual User Payer { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;
}
