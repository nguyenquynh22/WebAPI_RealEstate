using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Message
{
    public long MessageId { get; set; }

    public string ConversationId { get; set; } = null!;

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public string? ContentType { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
