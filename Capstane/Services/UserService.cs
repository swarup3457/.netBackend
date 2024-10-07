using CapstoneDAL; // Make sure to reference your DAL project
using CapstoneDAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstane.Services
{
	public class UserService
	{
		private readonly CapstoneDbContext _context;

		public UserService(CapstoneDbContext context)
		{
			_context = context;
		}

		// Create a new user
		public async Task<UserDetails> CreateUserAsync(UserDetails user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public UserDetails AddUser(UserDetails userDetails)
		{
			var existingUser = _context.Users.SingleOrDefault(u => u.Email == userDetails.Email);

			if (existingUser != null)
			{
				throw new Exception($"User already exists with email: {userDetails.Email}");
			}

			_context.Users.Add(userDetails);
			_context.SaveChanges();

			return userDetails;
		}

		// Get all users
		public async Task<List<UserDetails>> GetAllUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}

		// Get user by ID
		public async Task<UserDetails> GetUserByIdAsync(long id)
		{
			return await _context.Users.FindAsync(id);
		}

		// Update user
		public async Task<UserDetails> UpdateUserAsync(UserDetails user)
		{
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
			return user;
		}

		// Delete user
		public async Task<bool> DeleteUserAsync(long id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return false;

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return true;
		}
	}

}
