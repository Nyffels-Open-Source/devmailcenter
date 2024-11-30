﻿using DevMailCenter.Models;
using DevMailCenter.Repository;
using DevMailCenter.Security;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IMicrosoftLogic
{
    Guid Send(MicrosoftSettings settings, Email email);
}

public class MicrosoftLogic : IMicrosoftLogic
{
    private readonly ILogger<MicrosoftLogic> _logger;
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailTransactionRepository _emailTransactionRepository;
    private readonly IEncryptionLogic _encryptionLogic;
    
    public MicrosoftLogic(ILogger<MicrosoftLogic> logger, IEmailRepository emailRepository, IEmailTransactionRepository emailTransactionRepository, IEncryptionLogic encryptionLogic)
    {
        _logger = logger;
        _emailRepository = emailRepository;
        _emailTransactionRepository = emailTransactionRepository;
        _encryptionLogic = encryptionLogic;
    }

    public Guid Send(MicrosoftSettings settings, Email email)
    {
        // TODO
        return Guid.NewGuid();
    }
}