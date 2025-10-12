using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Users
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage ="First  Error"),MaxLength(50)]
        public string FirstName {  get; set; }

        [Required(ErrorMessage ="Last Error")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email Fromat")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required,MaxLength(11)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }


        public bool IsAdmin { get; set; }

    }
}
