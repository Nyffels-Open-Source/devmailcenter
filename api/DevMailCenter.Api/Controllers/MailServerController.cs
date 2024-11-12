using DevMailCenter.Logic;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("mailserver")]
    public class MailServerController : ControllerBase
    {
        private readonly ILogger<MailServerController> _logger;
        private readonly IMailServerLogic _mailServerLogic;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MailServerController(ILogger<MailServerController> logger, MailServerLogic mailServerLogic, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _mailServerLogic = mailServerLogic;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        [Route("{guid}")]
        [ProducesResponseType(typeof(MailServer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMailServer([FromRoute] Guid guid)
        {
            var mailServerRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetService<IMailServerRepository>();
            var mailServer = mailServerRepository.Get(guid);

            if (mailServer == null)
            {
                return NotFound();
            }
            
            return Ok(mailServer);
        }
    }
}