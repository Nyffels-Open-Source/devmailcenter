namespace DevMailCenter.Models;

public class EmailAttachment
{
    public Guid Id { get; set; }
    public Guid EmailId { get; set; }
    public string Mime { get; set; }
    public string Name { get; set; }
}

public class EmailAttachmentCreate
{
    public Guid EmailId { get; set; }
    public string Mime { get; set; }
    public string Name { get; set; }
    public string Base64 { get; set; }
}