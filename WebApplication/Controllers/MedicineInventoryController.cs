using AutoMapper;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Repositories;
using System.ComponentModel.DataAnnotations;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class MedicineInventoryController : Controller
	{
		private IMapper mapper;
		private MedicineInventoryRepository medicineInventoryRepository;
		private MedicineRepository medicineRepository;
		public MedicineInventoryController(IMapper mapper,
			MedicineInventoryRepository medicineInventoryRepository,
			MedicineRepository medicineRepository)
		{

			this.mapper = mapper;
			this.medicineInventoryRepository = medicineInventoryRepository;
			this.medicineRepository = medicineRepository;
		}
		public async Task<IActionResult> Index()
		{
			var dataRaw = await medicineInventoryRepository.GetAllMedicineInventory();
			List<MedicineInventoryModel> model = new List<MedicineInventoryModel>();
			foreach (var item in dataRaw)
			{
				model.Add(mapper.Map<MedicineInventoryModel>(item));
			}
			return View(model);
		}

		public async Task<IActionResult> Create()
		{
			var rawData = await medicineRepository.GetAllMedicine();
			List<MedicineModel> model = new List<MedicineModel>();
			foreach (var item in rawData)
			{
				model.Add(mapper.Map<MedicineModel>(item));
			}
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Create(MedicineInventoryModel model)
		{
			if (ModelState.IsValid)
			{
				MedicineInventory item = mapper.Map<MedicineInventory>(model);
				await medicineInventoryRepository.InsertMedicineInvetory(item);
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await medicineInventoryRepository.Delete(id);
			return RedirectToAction("Index");
		}
	}
}