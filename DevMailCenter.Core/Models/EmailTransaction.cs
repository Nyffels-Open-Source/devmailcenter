namespace DevMailCenter.Models;

public class EmailTransaction
{
    public Guid Id { get; set; }
    public Guid EmailId { get; set; }
    public string RawResponse { get; set; }
    public DateTime Created { get; set; }
}