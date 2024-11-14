using System.Net.Mail;

namespace DevMailCenter.Models;

public class Email
{
    public required Guid Id { get; set; }
    public required Guid ServerId { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
    public required MailPriority Priority { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? SendRequested { get; set; }
    public DateTime? Completed { get; set; }
    public required EmailStatus Status { get; set; }
    
    public ICollection<EmailReceiver> Receivers { get; set; }
}

public class EmailCreate
{
    public required string Subject { get; set; }
    public required string Message { get; set; }
    public required MailPriority Priority { get; set; }
    public required List<EmailReceiverCreate> Receivers { get; set; }
}

public class EmailUpdate
{
    public required string Subject { get; set; }
    public required string Message { get; set; }
    public required MailPriority Priority { get; set; }
    
    public List<Guid> DeletedReceivers { get; set; }
    public List<EmailReceiverUpdate> UpdatedReceivers { get; set; }
    public List<EmailReceiverCreate> CreatedReceivers { get; set; }
}

public enum EmailStatus
{
    Concept, 
    Pending, 
    Sent, 
    Failed
}