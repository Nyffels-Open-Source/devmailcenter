using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("mailserver")]
    public class MailServerController : ControllerBase
    {
        private readonly ILogger<SoftwareController> _logger;

        public MailServerController(ILogger<SoftwareController> logger)
        {
            _logger = logger;
        }
    }
}