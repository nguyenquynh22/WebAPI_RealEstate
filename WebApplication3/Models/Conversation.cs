using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Conversation
{
    public string ConversationId { get; set; } = null!;

    public Guid Participant1Id { get; set; }

    public Guid Participant2Id { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User Participant1 { get; set; } = null!;

    public virtual User Participant2 { get; set; } = null!;
}
