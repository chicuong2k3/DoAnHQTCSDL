using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class MedicineController : Controller
	{
		public IActionResult Index()
		{
			var model = new List<MedicineModel>() 
			{ 
				new MedicineModel()
				{
					Name = "Allerphast",
					Prescription = "Allerphast 180 mg là một sản phẩm của công ty cổ phần dược phẩm và sinh học y tế Mebiphar, thành phần chính chứa fexofenadin hydroclori",
					Quantity = 180,
					Unit = "viên"
				},
				new MedicineModel()
				{
					Name = "Allerphast",
					Prescription = "Allerphast 180 mg là một sản phẩm của công ty cổ phần dược phẩm và sinh học y tế Mebiphar, thành phần chính chứa fexofenadin hydroclori",
					Quantity = 180,
					Unit = "viên"
				}
			};
			return View(model);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(CreateMedicineModel model)
		{
			return View();
		}

		public IActionResult Delete()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
			return View();
		}
	}
}
