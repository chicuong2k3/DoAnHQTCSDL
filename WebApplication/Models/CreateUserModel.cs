using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class CreateUserModel
    {
		[Display(Name = "User Name")]
		[Remote(action: "IsAccountAvailable", controller: "Account")]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare(nameof(Password), ErrorMessage = "Confirm password does not match the password")]
		public string ConfirmPassword { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "User Type")]
        public string UserType { get; set; }
	}
}
