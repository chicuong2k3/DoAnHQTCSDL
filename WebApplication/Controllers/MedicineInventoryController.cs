using AutoMapper;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
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
			var listItem = new List<SelectListItem>();
			foreach (var item in rawData)
			{
				listItem.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString()});
			}
			ViewBag.ListMidicine = listItem;
			return View();
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
			var result = await medicineInventoryRepository.Delete(id);
			if(result == 1)
			{
				return Ok();
			}
			return BadRequest();
		}

		public async Task<IActionResult> Edit(int id)
		{
			var rawData = await medicineRepository.GetAllMedicine();
			var listItem = new List<SelectListItem>();
			foreach (var item in rawData)
			{
				listItem.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
			}
			ViewBag.ListMidicine = listItem;
			ViewBag.id = id;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(MedicineInventoryModel model)
		{
			if(ModelState.IsValid)
			{
				await medicineInventoryRepository.Update(mapper.Map<MedicineInventory>(model));
				return RedirectToAction("Index");
			}
			return View(model);
		}

	}
}