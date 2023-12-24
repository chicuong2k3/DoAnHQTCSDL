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
		private MedicalRecordRespository medicalRecordRespository;
		private AppDbContext appDbContext;
		public PaymentController(MedicalRecordRespository medicalRecordRespository, AppDbContext appDbContext)
		{	
			this.medicalRecordRespository = medicalRecordRespository;
			this.appDbContext = appDbContext;
		}
		public IActionResult Index()
		{
			return View();
		}


		public IActionResult ViewMedicalRecord()
		{
			return View();
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
