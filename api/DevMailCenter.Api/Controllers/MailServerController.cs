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
        public readonly IServiceScopeFactory _serviceScopeFactory;

        public MailServerController(ILogger<MailServerController> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        [Route("{guid}")]
        [ProducesResponseType(typeof(MailServer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMailServer([FromRoute] Guid guid)
        {
            var mailServer = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailServerRepository>().Get(guid);

            if (mailServer == null)
            {
                return NotFound();
            }

            return Ok(mailServer);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateMailServer([FromBody] MailServerCreate mailServer)
        {
            try
            {
                var mailServerResult = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailServerLogic>().CreateMailServer(mailServer);

                if (mailServerResult == null)
                {
                    return BadRequest("Something whent wrong. No data has been returned after creation.");
                }

                return Ok(mailServerResult);
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Duplicate mail server name" => Conflict(ex.Message),
                    _ => BadRequest(ex.Message)
                };
            }
        }
    }
}