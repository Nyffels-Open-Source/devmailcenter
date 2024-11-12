using devmailcenter.db;
using DevMailCenter.Models;

namespace DevMailCenter.Repository;

public interface IMailServerRepository
{
    MailServer Get(Guid id);
}

public class MailServerRepository : IMailServerRepository
{
    private readonly DmcContext _dbContext;

    public MailServerRepository(DmcContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MailServer? Get(Guid id)
    {
        return _dbContext.MailServers.Where(e => e.Id == id).FirstOrDefault();
    }
}