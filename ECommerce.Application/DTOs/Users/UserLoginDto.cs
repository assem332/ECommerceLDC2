using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Users
{
    public class UserLoginDto
    {

        [Required]
        [EmailAddress(ErrorMessage = "Email Fromat")]
        public string Email {  get; set; }


        [Required]
        public string Password { get; set; }
    }
}
