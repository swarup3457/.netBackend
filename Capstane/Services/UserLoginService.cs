using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CapstoneDAL;
using CapstoneDAL.Models;
using CapstoneDAL.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Capstane.Services
{
	public class UserLoginService
	{
		private readonly CapstoneDbContext _context;
		private readonly SymmetricSecurityKey _secretKey;
		private readonly double _expirationTimeInMinutes = 30;

		public UserLoginService(CapstoneDbContext context)
		{
			_context = context;
			_secretKey = GenerateKey(); // Call to key generation method
		}

		// Generate a new key
		private SymmetricSecurityKey GenerateKey()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				byte[] keyBytes = new byte[32]; // 256 bits key length
				rng.GetBytes(keyBytes);
				return new SymmetricSecurityKey(keyBytes);
			}
		}

		// Authenticate user credentials
		public async Task<UserDetails> AuthenticateAsync(string email, string password)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

			// Here you should hash the password and compare it to the stored hash
			if (user == null || !VerifyPassword(user.Password, password))
			{
				return null; // Return null if user not found or password doesn't match
			}

			return user;
		}

		// Verify the password (you will need to implement the hashing logic)
		private bool VerifyPassword(string storedHash, string password)
		{
			// Implement your password verification logic (e.g., hashing and comparing)
			return storedHash == password; // Example, replace with actual logic
		}

		// Find user by ID
		public async Task<UserDetails> FindUserAsync(long id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				throw new KeyNotFoundException($"User not found with ID: {id}");
			}
			return user;
		}

		// Generate JWT token
		public string GenerateToken(string email, string role, long userId)
		{
			var credentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

			// Use simple claim names for easier decoding
			var claims = new[]
			{
		new Claim("email", email),
		new Claim("role", role),
		new Claim("userId", userId.ToString()),
		new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
	};

			var token = new JwtSecurityToken(
				issuer: "YourApp",
				audience: "YourAppAudience",
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		// Login user and return token
		public async Task<LoginResponseDto> LoginAsync(string email, string password)
		{
			var user = await AuthenticateAsync(email, password);

			if (user != null)
			{
				var token = GenerateToken(email, user.Role, user.Id);
				return new LoginResponseDto(token); // Return the token in a DTO
			}

			throw new UnauthorizedAccessException("Invalid email or password"); // More specific exception
		}

	}
}
