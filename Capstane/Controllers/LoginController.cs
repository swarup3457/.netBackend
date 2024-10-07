using Capstane.Services;
using CapstoneDAL.Models;
using CapstoneDAL.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstane.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("AllowAllOrigins")]
	public class LoginController : ControllerBase
	{

		private readonly UserService _userService;
		public UserLoginService _UserLoginService;
		private readonly ILogger<LoginController> _logger;

		// Keep only one constructor
		public LoginController(UserLoginService userLoginService, ILogger<LoginController> logger, UserService userService)
		{
			_UserLoginService = userLoginService;
			_logger = logger;
			_userService = userService;
		}


		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
		{
			try
			{
				var loginResponse = await _UserLoginService.LoginAsync(loginRequest.Email, loginRequest.Password);
				_logger.LogInformation("Login successful for user: {Email}", loginRequest.Email);

				return Ok(loginResponse);
			}
			catch (Exception ex)
			{
				return Unauthorized(new LoginResponseDto( ex.Message));
			}
		}


		[HttpPost("add")]
		public IActionResult AddUser([FromBody] UserDetails userDetails)
		{
			try
			{
				_userService.AddUser(userDetails);
				return StatusCode(201, "User added successfully");
			}
			catch (Exception e)
			{
				return Conflict(e.Message);
			}
		}
	}
}
