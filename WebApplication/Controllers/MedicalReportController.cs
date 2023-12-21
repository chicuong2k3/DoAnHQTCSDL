using AutoMapper;
using DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MedicalRecordController : Controller
    {
        private IMapper mapper;
        private MedicalRecordRespository medicalRecordRespository;
        private UserManager<AppUser> userManager;
        private CustomerRepository customerRepository;

        public MedicalRecordController(IMapper mapper, 
            MedicalRecordRespository medicalRecordRespository,
            CustomerRepository customerRepository)
        {
            this.mapper = mapper;
            this.medicalRecordRespository = medicalRecordRespository;
            this.customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            var model = await medicalRecordRespository.GetByIdDentist(id);
            return View(model);
        }

        public async Task<IActionResult> AddMedicalRecord(string id)
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
        public async Task<IActionResult> AddMedicalRecord(MedicalRecordModel model)
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
                var recordMedical = await medicalRecordRespository.GetLatestMedicalRecordByCustomerId(item.CustomerId);
                if(recordMedical == null)
                {
                    var medicalRecord = await medicalRecordRespository.GetMaxId();
                    if(medicalRecord != null)
                    {
                        item.Id = medicalRecord.Id + 1;
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
                await medicalRecordRespository.Add(item);
                return Redirect($"/MedicalRecord/Index/{model.CreatedByDentistId}");
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
