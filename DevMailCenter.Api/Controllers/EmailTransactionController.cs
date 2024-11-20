using DevMailCenter.Logic;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("api/emailtransaction")]
    public class EmailTransactionController : ControllerBase
    {
        private readonly ILogger<EmailTransactionController> _logger;
        public readonly IServiceScopeFactory _serviceScopeFactory;

        public EmailTransactionController(ILogger<EmailTransactionController> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        [Route("{id}")]
        [EndpointName("GetEmailTransaction")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointDescription("Retrieve an email transaction by its ID.")]
        public IActionResult GetEmailTransaction([FromRoute] Guid id)
        {
            var transaction = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IEmailTransactionRepository>().Get(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
    }
}