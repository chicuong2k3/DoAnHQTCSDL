using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	public class ScheduleController : Controller
	{
		private readonly DentistRepository dentistRepository;
        private readonly AppointmentScheduleRepository appointmentScheduleRepository;

        public ScheduleController(DentistRepository dentistRepository,
            AppointmentScheduleRepository appointmentScheduleRepository)
        {
			this.dentistRepository = dentistRepository;
            this.appointmentScheduleRepository = appointmentScheduleRepository;
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAvailableDentists(DateTime date, DateTime startTime, DateTime endTime)
		{
            DateTime sTime = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    startTime.Hour,
                    startTime.Minute,
                    startTime.Second
                );
            DateTime eTime = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                endTime.Hour,
                endTime.Minute,
                endTime.Second
            );
            var dentists = await dentistRepository.GetAllAsync();
			var availableDentists = new List<Dentist>();
            foreach (var dentist in dentists)
			{
				bool isAvailable = true;
                var schedules = await appointmentScheduleRepository.GetAppointmentsOfDentist(dentist.Id);
				foreach (var schedule in schedules)
				{
					if ((sTime >= schedule.StartTime && sTime <= schedule.EndTime)
						|| (eTime >= schedule.StartTime && eTime <= schedule.EndTime))
					{
						isAvailable = false;
						break;
                    }
				}
				if (isAvailable)
				{
					availableDentists.Add(dentist);
				}
            }
            return Ok(availableDentists);
		}
		[Authorize(Roles = "Customer")]
        public async Task<IActionResult> MakeAppointment(string customerId)
		{
			ViewData["CustomerId"] = customerId;
			
            return View();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
		public async Task<IActionResult> MakeAppointment(CreateAppointmentModel model)
		{
			if (ModelState.IsValid)
			{
				var appointment = new AppointmentSchedule()
				{
					DentistId = model.DentistId,
					CustomerId = model.CustomerId
				};
				DateTime sTime = new DateTime(
					model.Date.Year,
					model.Date.Month,
					model.Date.Day,
					model.StartTime.Hour,
					model.StartTime.Minute,
					model.StartTime.Second
				);
                DateTime eTime = new DateTime(
                    model.Date.Year,
                    model.Date.Month,
                    model.Date.Day,
                    model.EndTime.Hour,
                    model.EndTime.Minute,
                    model.EndTime.Second
                );

				appointment.StartTime = sTime;
				appointment.EndTime = eTime;

                await appointmentScheduleRepository.AddAppoinment(appointment);
				return RedirectToAction("Index", "Home");
			}
			return View("ReviewAppointment", model);
		}
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> ReviewAppointment(CreateAppointmentModel model)
        {
			var dentist = await dentistRepository.GetDentistByIdAsync(model.DentistId);

            ViewData["DentistName"] = dentist.FullName;
            return View(model);
        }

		[Authorize(Roles = "Dentist")]
		[HttpGet]
		public async Task<IActionResult> GetAppointmentsByDate(string dentistId, DateTime date)
		{
			try
			{
				ViewData["ScheduleDate"] = date;
                var scheduleList = await appointmentScheduleRepository
                .GetAllSchedulesBelongToADentist(dentistId);
				var list = scheduleList
					.Where(x =>
					x.StartTime.Year == date.Year
					&& x.StartTime.Month == date.Month
					&& x.StartTime.Day == date.Day).ToList();

                return PartialView("_ListSchedulePartial", list);

			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[Authorize(Roles = "Dentist")]
        [HttpGet]
        public async Task<IActionResult> ListAppointmentSchedules(string dentistId)
        {
			ViewData["DentistId"] = dentistId;
			return View();
        }
    }
}
