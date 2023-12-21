using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Repositories;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class PaymentController : Controller
	{
		private MedicalReportRespository medicalReportRespository;
		private AppDbContext appDbContext;
		public PaymentController(MedicalReportRespository medicalReportRespository, AppDbContext appDbContext)
		{	
			this.medicalReportRespository = medicalReportRespository;
			this.appDbContext = appDbContext;
		}
		public IActionResult MedicalReport()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> MedicalReport(PhoneModel model)
		{
			if(ModelState.IsValid)
			{
				MedicalRecord? res = await medicalReportRespository.GetMedicalReportByPhoneCustomer(model.PhoneNumber);
				if(res != null)
				{
					TempData["medicalReport"] = res;
					return RedirectToAction("ViewMedicalReport");
				}
				else
				{
					ModelState.AddModelError(string.Empty, $"{model.PhoneNumber} not exists");
					return View("error");
				}
			}
			return View(model);
		}

		public IActionResult ViewMedicalReport()
		{
			MedicalRecord record = (MedicalRecord)TempData["medicalReport"];
			return View(record);
		}

		public async Task<IActionResult> Bill(string customerId)
		{
			BillModel model = new BillModel();	
			var medicalRecord = await appDbContext.MedicalRecords.
				Where(mr => mr.CustomerId == customerId).OrderByDescending(mr => mr.SequenceNumber).FirstOrDefaultAsync();
			if(medicalRecord != null)
			{
				var getListMedicine = await appDbContext.Medicine_MedicalRecords
					.Where(item => (item.MedicalRecordId == medicalRecord.Id && item.SequenceNumber == medicalRecord.SequenceNumber))
					.Select(item => new
					{
						quantity = item.MedicineQuantity,
						medicine = appDbContext.Medicines.Where(m => m.Id.Equals(item.MedicineId)).FirstOrDefault()
					})
					.ToListAsync();
				//create bill model
				model.NameService = medicalRecord.Service;
				model.SerVicePrice = medicalRecord.ServicePrice;
				decimal Total = model.SerVicePrice;
				foreach (var item in getListMedicine)
				{
					model.Medicines.Add(new MyMedicine()
					{
						Name = item.medicine.Name,
						Quantity = item.quantity,
						Price = item.medicine.Price
					});
					Total += item.quantity * item.medicine.Price;
				}
				model.Total = Total;
			}

			return View(model);
		}
    }
}
