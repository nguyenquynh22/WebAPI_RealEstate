using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class ConsultationRequest
{
    public Guid RequestId { get; set; }

    public Guid UserId { get; set; }

    public Guid? AssignedAgentId { get; set; }

    public string? ServiceType { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Inquiry { get; set; }

    public string? Status { get; set; }

    public DateTime? ClaimedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? AssignedAgent { get; set; }

    public virtual User User { get; set; } = null!;
}
