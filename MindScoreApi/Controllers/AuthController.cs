using Microsoft.AspNetCore.Mvc;
using MindScoreApi.Dtos;
using MindScoreApi.Services;

namespace MindScoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IAuthService _authService;

        public AuthController(IValidationService validationService, IAuthService authService)
        {
            _validationService = validationService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Validate input
            var validationResult = _validationService.ValidateRegisterRequest(
                request.Name,
                request.Email,
                request.Password,
                request.Age,
                request.Gender);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = validationResult.Message,
                    statusCode = 400
                });
            }

            // Register user
            var (success, message, user) = await _authService.RegisterAsync(
                request.Name,
                request.Email,
                request.Password,
                request.Age,
                request.Gender);

            if (!success)
            {
                return StatusCode(500, new
                {
                    message = message,
                    statusCode = 500
                });
            }

            return Ok(new
            {
                message = message,
                statusCode = 200,
                data = new
                {
                    userId = user?.Id,
                    name = user?.Name,
                    email = user?.Email
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Validate input
            var validationResult = _validationService.ValidateLoginRequest(
                request.Email,
                request.Password);

            if (!validationResult.IsValid)
            {
                return Unauthorized(new
                {
                    message = validationResult.Message,
                    statusCode = 401
                });
            }

            // Login user
            var (success, message, user) = await _authService.LoginAsync(
                request.Email,
                request.Password);

            if (!success)
            {
                return NotFound(new
                {
                    message = message,
                    statusCode = 404
                });
            }

            return Ok(new
            {
                message = message,
                statusCode = 200,
                data = new
                {
                    userId = user?.Id,
                    name = user?.Name,
                    email = user?.Email
                }
            });
        }
    }
}
