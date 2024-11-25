using DevMailCenter.Models;
using Microsoft.AspNetCore.Mvc;

namespace devmailcenterApi.Controllers
{
    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        public readonly IConfiguration _configuration;

        public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("providers/enabled")]
        [EndpointName("ListEnableProviders")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [EndpointDescription("Retrieve all the enabled providers for a email server")]
        public IActionResult listEnabledProviders()
        {
            var types = new List<MailServerType>();

            if (_configuration.GetSection("Smtp").GetValue<bool>("Enabled"))
            {
                types.Add(MailServerType.Smtp);
            }

            if (_configuration.GetSection("Microsoft").GetValue<bool>("Enabled"))
            {
                types.Add(MailServerType.MicrosoftExchange);
            }

            if (_configuration.GetSection("Google").GetValue<bool>("Enabled"))
            {
                types.Add(MailServerType.Google);
            }

            return Ok(types);
        }
    }
}