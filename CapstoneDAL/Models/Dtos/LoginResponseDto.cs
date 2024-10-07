using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneDAL.Models.Dtos
{
	public class LoginResponseDto
	{
		public string Token { get; set; }

		// Constructor
		public LoginResponseDto(string token)
		{
			Token = token;
		}
	}
}
