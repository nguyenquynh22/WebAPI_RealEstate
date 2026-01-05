using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Bio { get; set; }

    public bool? IsLocked { get; set; }

    public string? KycStatus { get; set; }

    public string? IdentityDocumentUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AgentCommission> AgentCommissions { get; set; } = new List<AgentCommission>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<ConsultationRequest> ConsultationRequestAssignedAgents { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<ConsultationRequest> ConsultationRequestUsers { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<Contract> ContractBuyers { get; set; } = new List<Contract>();

    public virtual ICollection<Contract> ContractSellers { get; set; } = new List<Contract>();

    public virtual ICollection<Conversation> ConversationParticipant1s { get; set; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationParticipant2s { get; set; } = new List<Conversation>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    public virtual ICollection<Transaction> TransactionPayers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionReceivers { get; set; } = new List<Transaction>();
}
