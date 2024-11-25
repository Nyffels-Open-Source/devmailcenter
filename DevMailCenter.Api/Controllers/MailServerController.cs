using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("api/mailserver")]
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
        [Route("{id}")]
        [EndpointName("GetMailServer")]
        [ProducesResponseType(typeof(MailServer), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointDescription("Retrieve an email server by its ID.")]
        public IActionResult GetMailServer([FromRoute] Guid id, [FromQuery] bool includeSettings = false)
        {
            var mailServer = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IMailServerRepository>().Get(id, includeSettings);

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
        [EndpointDescription("Retrieve all email servers.")]
        public IActionResult ListMailServer([FromQuery] bool includeSettings = false)
        {
            var mailServers = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IMailServerRepository>().List(includeSettings);

            return Ok(mailServers);
        }
        
        [HttpPost]
        [Route("smtp")]
        [EndpointName("CreateSmtpMailServer")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointDescription("Create a new SMTP email server. The endpoint will return the ID of the newly created email server.")]
        public async Task<IActionResult> CreateMailServer([FromBody] SmtpMailServerCreate mailServer)
        {
            try
            {
                var mailServerResult = _serviceScopeFactory.CreateScope().ServiceProvider
                    .GetRequiredService<IMailServerRepository>().CreateSmtp(mailServer);

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
                    _ => BadRequest(ex.Message)
                };
            }
        }
        
        [HttpPost]
        [Route("microsoft")]
        [EndpointName("CreateMicrosoftMailServer")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointDescription("Create a new Microsoft email server. The endpoint will return the ID of the newly created email server.")]
        public async Task<IActionResult> CreateMicrosoftMailServer([FromBody] MicrosoftMailServerCreate mailServer)
        {
            try
            {
                var mailServerResult = _serviceScopeFactory.CreateScope().ServiceProvider
                    .GetRequiredService<IMailServerRepository>().CreateMicrosoft(mailServer);

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
                    _ => BadRequest(ex.Message)
                };
            }
        }
        
        // [HttpPut]
        // [Route("{id}")]
        // [EndpointName("UpdateMailServer")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [EndpointDescription("Update an existing email server.")]
        // public IActionResult UpdateMailServer([FromRoute] Guid id, [FromBody] MailServerUpdate mailServer)
        // {
        //     try
        //     {
        //         _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailServerRepository>()
        //             .Update(id, mailServer);
        //
        //         return NoContent();
        //     }
        //     catch (Exception ex)
        //     {
        //         return ex.Message switch
        //         {
        //             "Mailserver not found" => NotFound(ex.Message),
        //             _ => BadRequest(ex.Message)
        //         };
        //     }
        // }
        
        [HttpDelete]
        [Route("{id}")]
        [EndpointName("DeleteMailServer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointDescription("Delete an existinge email server.")]
        public IActionResult DeleteMailServer([FromRoute] Guid id)
        {
            try
            {
                _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailServerRepository>()
                    .Delete(id);

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