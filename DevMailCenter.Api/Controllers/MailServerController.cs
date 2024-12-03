using DevMailCenter.External;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevMailCenter.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/mailserver")]
    public class MailServerController : ControllerBase
    {
        private readonly IMailServerRepository _mailServerRepository;
        private readonly IMicrosoftApi _microsoftApi;
        private readonly ILogger<MailServerController> _logger;
        private readonly IConfiguration _configuration;

        public MailServerController(ILogger<MailServerController> logger, IMailServerRepository mailServerRepository,
            IMicrosoftApi microsoftApi, IConfiguration configuration)
        {
            _logger = logger;
            _mailServerRepository = mailServerRepository;
            _microsoftApi = microsoftApi;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("{id}")]
        [EndpointName("GetMailServer")]
        [ProducesResponseType(typeof(MailServer), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointDescription("Retrieve an email server by its ID.")]
        public IActionResult GetMailServer([FromRoute] Guid id, [FromQuery] bool includeSettings = false)
        {
            var mailServer = _mailServerRepository.Get(id, includeSettings);

            if (mailServer == null)
            {
                return NotFound();
            }

            return Ok(mailServer);
        }

        [HttpGet]
        [Route("list")]
        [EndpointName("ListMailServers")]
        [ProducesResponseType(typeof(List<MailServer>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription("Retrieve all email servers.")]
        public IActionResult ListMailServer([FromQuery] bool includeSettings = false)
        {
            var mailServers = _mailServerRepository.List(includeSettings);

            return Ok(mailServers);
        }

        [HttpPost]
        [Route("smtp")]
        [EndpointName("CreateSmtpMailServer")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription(
            "Create a new SMTP email server. The endpoint will return the ID of the newly created email server.")]
        public async Task<IActionResult> CreateMailServer([FromBody] SmtpMailServerCreate mailServer)
        {
            if (_configuration["Smtp:Enabled"] == "False")
            {
                return Unauthorized("Smtp has been disabled");
            }

            try
            {
                var mailServerResult = _mailServerRepository.CreateSmtp(mailServer);
                return Ok(mailServerResult);
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    _ => BadRequest(ex.Message)
                };
            }
        }

        [HttpPost]
        [Route("microsoft")]
        [EndpointName("CreateMicrosoftMailServer")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription(
            "Create a new Microsoft email server. The endpoint will return the ID of the newly created email server.")]
        public IActionResult CreateMicrosoftMailServer([FromBody] MicrosoftMailServerCreate mailServer)
        {
            if (_configuration["Microsoft:Enabled"] == "False")
            {
                return Unauthorized("Microsoft has been disabled");
            }

            try
            {
                var mailServerResult = _mailServerRepository.CreateMicrosoft(mailServer);
                return Ok(mailServerResult);
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    _ => BadRequest(ex.Message)
                };
            }
        }

        [HttpGet]
        [Route("microsoft/authenticate")]
        [EndpointName("GetMicrosoftAuthenticationUrl")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription(
            "This endpoint will return a Microsoft Authentication Url needed to request the Access token for usage in CreateMicrosoftMailServer endpoint. The return url must be registered in the registed app inside azure portal.")]
        public async Task<IActionResult> GetMicrosoftAuthenticationUrl([FromQuery] string redirectUri)
        {
            try
            {
                var uri = _microsoftApi.GenerateAuthenticationRedirectUrl(redirectUri);
                return Ok(uri);
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    _ => BadRequest(ex.Message)
                };
            }
        }

        [HttpPut]
        [Route("microsoft/{id}")]
        [EndpointName("UpdateMicrosoftMailServer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateMicrosoftMailServer([FromRoute] Guid id,
            [FromBody] MicrosoftMailServerUpdate mailServer)
        {
            try
            {
                _mailServerRepository.UpdateMicrosoft(id, mailServer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Mailserver type isn't Microsoft Exchange" => NotFound(),
                    "Mailserver not found" => NotFound(),
                    _ => BadRequest(ex.Message)
                };
            }
        }
        
        [HttpPut]
        [Route("smtp/{id}")]
        [EndpointName("UpdateSmtpMailServer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateSmtpMailServer([FromRoute] Guid id,
            [FromBody] SmtpMailServerUpdate mailServer)
        {
            try
            {
                _mailServerRepository.UpdateSmtp(id, mailServer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Mailserver type isn't Smtp" => BadRequest(ex.Message),
                    "Mailserver not found" => NotFound(),
                    _ => BadRequest(ex.Message)
                };
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [EndpointName("DeleteMailServer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription("Delete an existinge email server.")]
        public IActionResult DeleteMailServer([FromRoute] Guid id)
        {
            try
            {
                _mailServerRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    _ => BadRequest(ex.Message)
                };
            }
        }
    }
}