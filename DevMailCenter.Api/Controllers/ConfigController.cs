using DevMailCenter.Logic;
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
        private readonly IEncryptionLogic _encryptionLogic;

        public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration, IEncryptionLogic encryptionLogic)
        {
            _logger = logger;
            _configuration = configuration;
            _encryptionLogic = encryptionLogic;
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
        
        [HttpGet]
        [Route("encryption/enabled")]
        [EndpointName("CheckEncryptionStatus")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [EndpointDescription("Retrieve the encrypton status")]
        public IActionResult RetrieveEncryptionStatus()
        {
            return Ok(_encryptionLogic.IsEncryptionEnabled());
        }
    }
}