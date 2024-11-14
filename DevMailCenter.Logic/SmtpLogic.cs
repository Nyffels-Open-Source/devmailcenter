using DevMailCenter.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace DevMailCenter.Logic;

public interface ISmtpLogic
{
    Guid Send(SmtpSettings settings, Email email);
}

public class SmtpLogic : ISmtpLogic
{
    private readonly ILogger<EmailLogic> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SmtpLogic(ILogger<EmailLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Guid Send(SmtpSettings settings, Email email)
    {
        /* Create the message */
        MailMessage mailMessage = new MailMessage();
        mailMessage.Body = email.Message;
        mailMessage.Subject = email.Subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Priority = email.Priority;
        email.Receivers.Where(receiver => receiver.Type == EmailReceiverType.To)
            .Select(receiver => new MailAddress(receiver.ReceiverEmail)).ToList()
            .ForEach(receiver => mailMessage.To.Add(receiver));
        email.Receivers.Where(receiver => receiver.Type == EmailReceiverType.CC)
            .Select(receiver => new MailAddress(receiver.ReceiverEmail)).ToList()
            .ForEach(receiver => mailMessage.CC.Add(receiver));
        email.Receivers.Where(receiver => receiver.Type == EmailReceiverType.BCC)
            .Select(receiver => new MailAddress(receiver.ReceiverEmail)).ToList()
            .ForEach(receiver => mailMessage.Bcc.Add(receiver));
        mailMessage.Sender = new MailAddress(settings.Email);

        /* Create the SMTP Client */
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = settings.Host;
        smtpClient.Port = settings.Port;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new System.Net.NetworkCredential(settings.User, settings.Password);
        smtpClient.EnableSsl = true;

        try
        {
            smtpClient.Send(mailMessage);
            Console.WriteLine("Email sent Successfully"); // TODO
            // TODO Save Transaction properties and return transaction id.
            // TODO Set email status
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // TODO
            // TODO Save Transacrion properties and return the transcation id. 
            // TODO Set email status
        }
        
        return Guid.NewGuid(); // TODO Delete, return will have to be the transaction id. 
    }
}