using DevMailCenter.Repository;

namespace DevMailCenter.Logic;

public interface IMailServerLogic
{
}

public class MailServerLogic : IMailServerLogic
{
    private readonly MailServerRepository mailServerRepository;

    public MailServerLogic(MailServerRepository _mailServerRepository)
    {
        mailServerRepository = _mailServerRepository;
    }
}