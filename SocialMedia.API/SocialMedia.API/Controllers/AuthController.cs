using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using SocialMedia.Models;
using SocialMedia.Data;

namespace ECommerce.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IRepository repo, ILogger<AuthController> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }

        [Route("auth/register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserDTO newUser)
        {
            _logger.LogInformation("auth/register triggered");
            try
            {
                int user = await _repo.CreateNewUserAndReturnUserIdAsync(newUser);
                _logger.LogInformation("auth/register completed successfully");
                return Ok(user);
            }
            catch
            {
                _logger.LogWarning("auth/register completed with errors");
                return BadRequest();
            }
        }


        [Route("auth/login")]
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody] LoginDTO LR)
        {
            _logger.LogInformation("auth/login triggered");
            try
            {
                User login = await _repo.GetUserLoginAsync(LR.password, LR.email);
                
                _logger.LogInformation("auth/login completed successfully");
                return Ok(login);
            }
            catch
            {
                _logger.LogWarning("auth/login completed with errors");
                return BadRequest();
            }
        }

        [Route("auth/logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            _logger.LogInformation("auth/logout triggered");
            _logger.LogInformation("auth/logout completed successfully");
            return Ok();
        }

    }
}
