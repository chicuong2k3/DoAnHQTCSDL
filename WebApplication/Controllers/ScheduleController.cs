using DataModels;
using Microsoft.AspNetCore.Mvc;
using Repositories;
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

		public async Task<IActionResult> GetAvailableDentists(DateTime startTime, int duration)
		{
			DateTime endTime = startTime.AddMinutes(duration);
			var dentists = await dentistRepository.GetAllAsync();
			var availableDentists = new List<Dentist>();
            foreach (var dentist in dentists)
			{
				bool isAvailable = true;
                var schedules = await appointmentScheduleRepository.GetAppointmentsOfDentist(dentist.Id);
				foreach (var schedule in schedules)
				{
					if ((startTime >= schedule.StartTime && startTime <= schedule.EndTime)
						|| (endTime >= schedule.StartTime && endTime <= schedule.EndTime))
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
        public async Task<IActionResult> MakeAppointment(string customerId)
		{
			ViewData["CustomerId"] = customerId;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> MakeAppointment(CreateAppointmentModel model)
		{
			if (ModelState.IsValid)
			{
				var appointment = new AppointmentSchedule()
				{
					StartTime = model.StartTime,
					EndTime = model.StartTime.AddMinutes(model.Duration),
					DentistId = model.DentistId,
					CustomerId = model.CustomerId
				};
				// Lỗi khóa ngoại
				await appointmentScheduleRepository.AddAppoinment(appointment);
			}
			return View();
		}
	}
}
