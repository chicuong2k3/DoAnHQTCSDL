using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
	public class PrescriptionController : Controller
	{
		public ActionResult Index(int id, int sequence)
		{
			return View();
		}
		public IActionResult Create(int id, int sequence)
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create()
		{
			return View();
		}
	}
}
