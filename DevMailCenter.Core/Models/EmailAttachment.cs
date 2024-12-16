namespace DevMailCenter.Models;

public class EmailAttachment
{
    public Guid Id { get; set; }
    public Guid EmailId { get; set; }
    public required string Mime { get; set; }
    public required string Name { get; set; }
}

public class EmailAttachmentCreate
{
    public required string Mime { get; set; }
    public required string Name { get; set; }
    public required string Base64 { get; set; }
}