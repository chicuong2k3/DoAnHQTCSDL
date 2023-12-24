using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Repositories;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class PaymentController : Controller
	{
		private MedicalRecordRespository medicalRecordRespository;
		private AppDbContext appDbContext;
		private CustomerRepository customerRepository;
		public PaymentController(MedicalRecordRespository medicalRecordRespository
			, AppDbContext appDbContext, CustomerRepository customerRepository)
		{	
			this.medicalRecordRespository = medicalRecordRespository;
			this.appDbContext = appDbContext;
			this.customerRepository = customerRepository;
		}
		public async Task<IActionResult> Index()
		{
			var result = await customerRepository.GetAll();
			var result1 = from c in result.ToList()
						  join m in appDbContext.MedicalRecords
						  on c.Id equals m.CustomerId into mytable
						  from item in mytable
						  where item.Status == "no"
						  select c;
			result1 = result1.Distinct().ToList();
			var list = new List<SelectListItem>();
			foreach (var item in result1)
			{
				list.Add(new SelectListItem()
				{
					Value = item.Id,
					Text = item.PhoneNumber
				});
			}
			ViewBag.list = list;
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
						medicine = appDbContext.Medicines.Where(m => m.Id == item.MedicineId).FirstOrDefault()
					}).ToListAsync();
				model.NameService = medicalRecord.Service;
				model.SerVicePrice = medicalRecord.ServicePrice;
				//create bill model
				if(getListMedicine == null || getListMedicine.Count == 0) {
					return View(model);
				}
				//update trang thai da thanh toan chi phi
                var temMr = medicalRecord;
                temMr.Status = "yes";
                appDbContext.MedicalRecords.Entry(medicalRecord).CurrentValues.SetValues(temMr);
				await appDbContext.SaveChangesAsync();
				//
                decimal Total = medicalRecord.ServicePrice;
				model.medicines = new List<MyMedicine>();
				foreach (var item in getListMedicine)
				{
					model.medicines.Add(new MyMedicine()
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
