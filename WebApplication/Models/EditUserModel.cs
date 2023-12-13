using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class EditUserModel
    {
        [Required]
        public string Id { get; set; }
        [Display(Name = "User Name")]
		[Remote(action: "IsAccountAvailable", controller: "Account")]
		public string UserName { get; set; }
		public IList<string>? Roles { get; set; }
	}
}
