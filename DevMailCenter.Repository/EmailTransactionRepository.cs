using DevMailCenter.Core;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Repository;

public interface IEmailTransactionRepository
{
    EmailTransaction Get(Guid id);
    Guid Create(Guid emailId, string response);
}

public class EmailTransactionRepository : IEmailTransactionRepository
{
    private readonly DmcContext _dbContext;

    public EmailTransactionRepository(DmcContext dbContext)
    {
        _dbContext = dbContext;
    }

    public EmailTransaction Get(Guid id)
    {
        return _dbContext.EmailTransactions.FirstOrDefault(x => x.Id == id);
    }
    
    public Guid Create(Guid emailId, string response)
    {
        var transaction = new EmailTransaction
        {
            EmailId = emailId,
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            RawResponse = response
        };
        _dbContext.EmailTransactions.Add(transaction);
        _dbContext.SaveChanges();
        
        return transaction.Id;
    }
}