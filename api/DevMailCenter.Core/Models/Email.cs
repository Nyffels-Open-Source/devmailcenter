﻿namespace DevMailCenter.Models;

public class Email
{
    public required Guid Id { get; set; }
    public required Guid ServerId { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? SendRequested { get; set; }
    public DateTime? Completed { get; set; }
    public required EmailStatus Status { get; set; }
    public string? RawServerResponse { get; set; }
    
    public ICollection<EmailReceiver> Receivers { get; set; }
}

public class EmailCreate
{
    public required string Subject { get; set; }
    public required string Message { get; set; }
}

public enum EmailStatus
{
    Concept, 
    Pending, 
    Sent, 
    Failed
}