using AutoMapper;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class MedicineController : Controller
	{
		private AppDbContext dbContext;
		private MedicineRepository medicineRepository;
		private IMapper mapper;
		public MedicineController (AppDbContext dbContext, MedicineRepository medicine, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.medicineRepository = medicine;
			this.mapper = mapper;
		}
		public async Task<IActionResult> Index()
		{
			var item = await medicineRepository.GetAllMedicine();
			if(item == null)
			{
				var newmodel = new List<CreateMedicineModel>()
				{
					new CreateMedicineModel()
					{
						Id = 1,
						Name = "Allerphast",
						Prescription = "Allerphast 180 mg là một sản phẩm của công ty cổ phần dược phẩm và sinh học y tế Mebiphar, thành phần chính chứa fexofenadin hydroclori",
					},
					new CreateMedicineModel()
					{
						Id = 2,
						Name = "Allerphast",
						Prescription = "Allerphast 180 mg là một sản phẩm của công ty cổ phần dược phẩm và sinh học y tế Mebiphar, thành phần chính chứa fexofenadin hydroclori"
					}
				};
				return View(newmodel);
			}
			List<CreateMedicineModel> model = new List<CreateMedicineModel> ();
			for (int i = 0; i < item.Count(); i++)
			{
				model.Add(mapper.Map<CreateMedicineModel>(item[i]));
			}
			return View(model);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateMedicineModel model)
		{
			if (ModelState.IsValid)
			{
				Medicine item = mapper.Map<Medicine>(model);
				await medicineRepository.AddMedicine(item);
				return RedirectToAction("Index");
			}
			return View(model);
		}

		public IActionResult Edit(int id)
		{
			ViewData["Id"] = id;
            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(CreateMedicineModel model)
		{
			if(ModelState.IsValid)
			{
				await medicineRepository.UpdateMedicine(mapper.Map<Medicine>(model));
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await medicineRepository.RemoveMedicine(id);
			return RedirectToAction("Index");
		}
	}
}
