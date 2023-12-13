using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
	public class ProductController : Controller
	{
		[Authorize]
		public IActionResult Index()
		{
			return View();
		}
	}
}
