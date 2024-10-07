using Capstane.Services;
using CapstoneDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstane.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserService _userService;

		public UsersController(UserService userService)
		{
			_userService = userService;
		}

		// POST: api/users
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] UserDetails user)
		{
			if (user == null)
			{
				return BadRequest("User cannot be null.");
			}

			var createdUser = await _userService.CreateUserAsync(user);
			return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
		}

		// GET: api/users
		[HttpGet]
		public async Task<ActionResult<List<UserDetails>>> GetAllUsers()
		{
			var users = await _userService.GetAllUsersAsync();
			return Ok(users);
		}

		// GET: api/users/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<UserDetails>> GetUserById(long id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		// PUT: api/users/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(long id, [FromBody] UserDetails user)
		{
			if (id != user.Id)
			{
				return BadRequest("User ID mismatch.");
			}

			var updatedUser = await _userService.UpdateUserAsync(user);
			if (updatedUser == null)
			{
				return NotFound();
			}

			return NoContent(); // Return 204 No Content
		}

		// DELETE: api/users/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(long id)
		{
			var result = await _userService.DeleteUserAsync(id);
			if (!result)
			{
				return NotFound(); // Return 404 Not Found if user doesn't exist
			}
			return NoContent(); // Return 204 No Content
		}
	}
}
