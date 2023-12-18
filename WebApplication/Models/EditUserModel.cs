using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class EditUserModel
    {
        [Required]
        public string Id { get; set; }
        [Display(Name = "User Name")]
		
		public string UserName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public bool IsLocked { get; set; }
    }
}
