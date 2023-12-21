using AutoMapper;
using DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repositories;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MedicalReportController : Controller
    {
        private IMapper mapper;
        private MedicalReportRespository medicalReportRespository;
        private UserManager<AppUser> userManager;
        private CustomerRepository customerRepository;

        public MedicalReportController(IMapper mapper, 
            MedicalReportRespository medicalReportRespository,
            CustomerRepository customerRepository)
        {
            this.mapper = mapper;
            this.medicalReportRespository = medicalReportRespository;
            this.customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            var model = await medicalReportRespository.GetByIdDentist(id);
            return View(model);
        }

        public async Task<IActionResult> AddMedicalReport(string id)
        {
            ViewBag.id = id;
            ViewBag.SelectService = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Khám tổng quát", Value = "Khám tổng quát"},
                new SelectListItem() {Text = "Nội soi", Value = "Nột soi"},
                new SelectListItem() {Text = "Siêu âm", Value = "Siêu âm"}
            };

            var SelectPhoneNumber = new List<SelectListItem>();
            var listPhone = await customerRepository.GetAll();
            foreach (var item in listPhone)
            {
                SelectPhoneNumber.Add(new SelectListItem() { Text = item.PhoneNumber, Value = item.PhoneNumber });
            }
            ViewBag.SelectPhoneNumber = SelectPhoneNumber;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicalReport(MedicalReportModel model)
        {
            if (ModelState.IsValid)
            {
                MedicalRecord item = new MedicalRecord();
                item.CreatedByDentistId = model.CreatedByDentistId;
                item.ExamDentistId = model.CreatedByDentistId;
                item.ExaminationDate = model.ExaminationDate;
                item.Service = model.Service;
                var targetCus = await customerRepository.GetCustomerByPhoneNumber(model.PhoneNumber.Trim());
                if(targetCus == null)
                {
                    ModelState.AddModelError(string.Empty, $"{model.PhoneNumber} không tồn tại");
                    return View("error");
                }
                item.CustomerId = targetCus.Id;
                item.ServicePrice = 100000;
                var recordMedical = await medicalReportRespository.GetLatestMedicalReportByCustomerId(item.CustomerId);
                if(recordMedical == null)
                {
                    var medicalReport = await medicalReportRespository.GetMaxId();
                    if(medicalReport != null)
                    {
                        item.Id = medicalReport.Id + 1;
                        item.SequenceNumber = 1;
                    }
                    else
                    {
                        item.Id = 1;
                        item.SequenceNumber = 1;
                    }
                }
                else
                {
                    item.Id = recordMedical.Id;
                    item.SequenceNumber = recordMedical.SequenceNumber + 1;
                }
                await medicalReportRespository.Add(item);
                return Redirect($"/MedicalReport/Index/{model.CreatedByDentistId}");
            }
            ViewBag.SelectService = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Khám tổng quát", Value = "Khám tổng quát"},
                new SelectListItem() {Text = "Nội soi", Value = "Nột soi"},
                new SelectListItem() {Text = "Siêu âm", Value = "Siêu âm"}
            };

            var SelectPhoneNumber = new List<SelectListItem>();
            var listPhone = await customerRepository.GetAll();
            foreach (var item in listPhone)
            {
                SelectPhoneNumber.Add(new SelectListItem() { Text = item.PhoneNumber, Value = item.PhoneNumber });
            }
            ViewBag.SelectPhoneNumber = SelectPhoneNumber;
            return View(model);
        }
    }
}
