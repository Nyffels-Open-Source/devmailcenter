using DevMailCenter.Logic;
using DevMailCenter.Models;
using DevMailCenter.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevMailCenter.Controllers
{
    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        public readonly IConfiguration _configuration;
        private readonly IEncryptionLogic _encryptionLogic;

        [HttpGet]
        [Route("status")]
        [EndpointName("GetApiStatus")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [EndpointDescription("Check if the API is alive")]
        public IActionResult getApiStatus()
        {
            return Ok("OK");
        }
        
        public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration, IEncryptionLogic encryptionLogic)
        {
            _logger = logger;
            _configuration = configuration;
            _encryptionLogic = encryptionLogic;
        }

        [Authorize]
        [HttpGet]
        [Route("providers/enabled")]
        [EndpointName("ListEnableProviders")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        
        [Authorize]
        [HttpGet]
        [Route("encryption/enabled")]
        [EndpointName("CheckEncryptionStatus")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription("Retrieve the encrypton status")]
        public IActionResult RetrieveEncryptionStatus()
        {
            return Ok(_encryptionLogic.IsEncryptionEnabled());
        }
        
        [Authorize]
        [HttpPost]
        [Route("encryption/generate-key")]
        [EndpointName("GenerateNewEncryptionKey")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EndpointDescription("Generate a new encryption key. Use the query 'updateSensitiveData' to update all the sensitive data in the system to the new key, decrypting old data will happen with the current set key. Be aware, generating a key and updating sensitive data will allow the new key to become active.")]
        public IActionResult GenerateEncryptionKey([FromQuery] bool updateSensitiveData = true)
        {
            return Ok(_encryptionLogic.GenerateEncryptionKey(updateSensitiveData));
        }
    }
}