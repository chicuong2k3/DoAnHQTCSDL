using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class RegisterModel
    {
        [Display(Name = "Phone Number")]
        //[Remote(action: "IsAccountAvailable", controller: "Account")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Confirm password does not match the password")]
        public string ConfirmPassword { get; set; }
		public string FullName { get; set; }
		public string Address { get; set; }
        [DataType(DataType.Date)]
		public DateTime DayOfBirth { get; set; }
	}
}
