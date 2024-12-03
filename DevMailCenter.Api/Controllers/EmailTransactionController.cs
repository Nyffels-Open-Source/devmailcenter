using DevMailCenter.Logic;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevMailCenter.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/emailtransaction")]
    public class EmailTransactionController : ControllerBase
    {
        private readonly ILogger<EmailTransactionController> _logger;
        private readonly IEmailTransactionRepository _emailTransactionRepository;

        public EmailTransactionController(ILogger<EmailTransactionController> logger, IEmailTransactionRepository emailTransactionRepository)
        {
            _logger = logger;
            _emailTransactionRepository = emailTransactionRepository;
        }

        [HttpGet]
        [Route("{id}")]
        [EndpointName("GetEmailTransaction")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointDescription("Retrieve an email transaction by its ID.")]
        public IActionResult GetEmailTransaction([FromRoute] Guid id)
        {
            var transaction = _emailTransactionRepository.Get(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
    }
}