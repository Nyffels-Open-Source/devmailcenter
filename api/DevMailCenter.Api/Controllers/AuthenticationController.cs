using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<SoftwareController> _logger;

        public AuthenticationController(ILogger<SoftwareController> logger)
        {
            _logger = logger;
        }
    }
}