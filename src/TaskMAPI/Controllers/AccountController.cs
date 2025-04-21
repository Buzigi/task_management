using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskMAPI.Models.Authentication;
using TaskMAPI.Services;

namespace TaskMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthenticationService signInManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string message;
            if (!ModelState.IsValid)
            {
                message = "Invalid login attempt.";
                _logger.LogWarning($"{message}. User: {model.Email}");
                ModelState.AddModelError(string.Empty, message);
                return BadRequest(ModelState);
            }

            var success = await _signInManager.LoginAsync(model.Email, model.Password);

            if (success)
            {
                message = $"User {model.Email} Logged in successfully";
                _logger.LogInformation(message);
                return Ok(message);
            }

            message = "Invalid login attempt.";
            _logger.LogWarning($"{message}. User: {model.Email}");
            ModelState.AddModelError(string.Empty, message);
            return BadRequest(ModelState);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string accessToken)
        {
            try
            {
                string message;
                if (string.IsNullOrEmpty(accessToken))
                {
                    message = "Access token is missing.";
                    _logger.LogWarning(message);
                    return BadRequest(message);
                }

                await _signInManager.LogoutAsync(accessToken);

                message = "Logged out successfully";
                _logger.LogInformation(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");

                return BadRequest(ex.Message);
            }
        }
    }
}
