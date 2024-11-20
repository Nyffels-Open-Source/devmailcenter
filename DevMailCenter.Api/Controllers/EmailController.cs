using DevMailCenter.Logic;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        public readonly IServiceScopeFactory _serviceScopeFactory;

        public EmailController(ILogger<EmailController> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        [Route("{id}")]
        [EndpointName("GetEmail")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointDescription("Retrieve an email by its ID.")]
        public IActionResult GetEmail([FromRoute] Guid id, [FromQuery] bool includeReceivers = false)
        {
            var email = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IEmailRepository>().Get(id, includeReceivers);

            if (email == null)
            {
                return NotFound();
            }

            return Ok(email);
        }
        
        [HttpGet]
        [Route("list")]
        [EndpointName("ListEmails")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        [EndpointDescription("Retrieve all emails.")]
        public IActionResult ListEmails([FromQuery] bool includeReceivers = false)
        {
            var mailServers = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IEmailRepository>().List(includeReceivers);

            return Ok(mailServers);
        }
        
        [HttpPost]
        [Route("{serverId}")]
        [EndpointName("CreateEmail")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointDescription("Create a new email server. The endpoint will return the ID of the newly created email server.")]
        public IActionResult CreateEmail([FromBody] EmailCreate email, [FromRoute] Guid serverId)
        {
            try
            {
                var emailResult = _serviceScopeFactory.CreateScope().ServiceProvider
                    .GetRequiredService<IEmailRepository>().Create(email, serverId);

                if (emailResult == null)
                {
                    return BadRequest("Something whent wrong. No data has been returned after creation.");
                }
                
                return Ok(emailResult);
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
        [Route("{id}")]
        [EndpointName("UpdateEmail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [EndpointDescription("Update an existing email. Be aware only e-mails in status 'Concept' can be edited. Other statusses are non-editable")]
        public IActionResult UpdateEmail([FromRoute] Guid id, [FromBody] EmailUpdate email)
        {
            try
            {
                _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailRepository>()
                    .Update(id, email);

                return NoContent();
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Email not found" => NotFound(ex.Message),
                    "Email isn't in an editable state" => Conflict(ex.Message),
                    _ => BadRequest(ex.Message)
                };
            }
        }
        
        [HttpDelete]
        [Route("{id}")]
        [EndpointName("DeleteEmail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointDescription("Delete an existinge email.")]
        public IActionResult DeleteEmail([FromRoute] Guid id)
        {
            try
            {
                _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailRepository>()
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
        
        [HttpPost]
        [Route("{emailId}/send")]
        [EndpointName("SendEmail")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [EndpointDescription("Send a e-mail. Be aware, only concept e-mails can be send.")]
        public IActionResult SendEmail([FromRoute] Guid emailId)
        {
            try
            {
                var transactionId = _serviceScopeFactory.CreateScope().ServiceProvider
                    .GetRequiredService<IEmailLogic>().Send(emailId);
                
                return Ok(transactionId);
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "Receivers are required to send the e-mail" => UnprocessableEntity(ex.Message),
                    "Email is not in 'concept' status." => Conflict(ex.Message),
                    "Email not found" => NotFound(ex.Message),
                    _ => BadRequest(ex.Message)
                };
            }
        }
    }
}