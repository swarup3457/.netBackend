using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneDAL.Models
{
	public class UserDetails
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate ID value
		public long Id { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string Role { get; set; }
		public UserDetails() { }

		

		public UserDetails(string email, string name, string password, string role)
		{
			Email = email;
			Name = name;
			Password = password;
			Role = role;
		}
	}

}
