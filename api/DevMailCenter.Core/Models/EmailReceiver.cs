namespace DevMailCenter.Models;

public class EmailReceiver
{
    public Guid Id { get; set; }
    public Guid EmailId { get; set; }
    public required string ReceiverEmail { get; set; }
    public required EmailReceiverType Type { get; set; }
}

public enum EmailReceiverType
{
    To, 
    CC, 
    BCC
}