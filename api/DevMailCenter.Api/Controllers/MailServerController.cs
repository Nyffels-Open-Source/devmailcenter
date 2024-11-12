using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("mailserver")]
    public class MailServerController : ControllerBase
    {
        private readonly ILogger<MailServerController> _logger;

        public MailServerController(ILogger<MailServerController> logger)
        {
            _logger = logger;
        }
    }
}