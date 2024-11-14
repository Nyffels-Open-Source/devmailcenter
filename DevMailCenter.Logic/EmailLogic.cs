using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IEmailLogic
{
    Guid Send(Guid emailId);
}

public class EmailLogic : IEmailLogic
{
    private readonly ILogger<EmailLogic> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public EmailLogic(ILogger<EmailLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Guid Send(Guid emailId)
    {
        var email = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailRepository>().Get(emailId);
        if (email is null)
        {
            throw new Exception("Email not found");
        }
        
        // TODO
        return Guid.NewGuid();
    }
}