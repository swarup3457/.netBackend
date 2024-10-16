using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneDAL.Models.Dtos
{
  

    public class UserDto

    {

        public UserDto()

        {

        }
 
        public UserDto(UserDetails user)

        {

            Id = user.Id;

            Email = user.Email;

            Name = user.Name;

        }
 
        public long Id { get; set; }

        public string Email { get; set; }
 
        public string Name { get; set; }

    }

}

 
