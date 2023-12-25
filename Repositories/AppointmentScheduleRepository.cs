using DataModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Repositories
{
	public class AppointmentScheduleRepository
	{
		private readonly AppDbContext dbContext;

		public AppointmentScheduleRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

        public async Task<List<AppointmentSchedule>> GetAppointmentsOfDentist(string dentistId)
		{
			return await dbContext.AppointmentSchedules
				.Where(x => x.DentistId == dentistId).ToListAsync();
		}

        public async Task<List<AppointmentSchedule>> GetAppointmentsOfCustomer(string customerId)
        {
            return await dbContext.AppointmentSchedules
                .Where(x => !string.IsNullOrEmpty(x.CustomerId) 
				&& x.CustomerId == customerId
				&& x.StartTime >= DateTime.Now)
				.ToListAsync();
        }

        public async Task AddAppoinment(AppointmentSchedule appointment)
        {
            await dbContext.AppointmentSchedules.AddAsync(appointment);
			await dbContext.SaveChangesAsync();	
        }

		public async Task<List<AppointmentSchedule>> GetAllSchedulesBelongToADentist(string dentistId)
		{
			return await dbContext.AppointmentSchedules
				.Where(x => x.DentistId == dentistId).ToListAsync();
		}

		public async Task<AppointmentSchedule> FindAsync(string dentistId, DateTime startTime)
		{
			return await dbContext.AppointmentSchedules
				.Where(x => x.DentistId == dentistId && x.StartTime == startTime).SingleOrDefaultAsync();
		}

		public async Task UpdateAsync(AppointmentSchedule schedule, DateTime sTime, DateTime eTime, string dentistId = null)
		{
			var newSchedule = new AppointmentSchedule()
			{
				StartTime = sTime,
				EndTime = eTime,
				CustomerId = schedule.CustomerId
			};
			if (dentistId == null)
			{
				newSchedule.DentistId = schedule.DentistId;
			}
			else
			{
				newSchedule.DentistId = dentistId;
			}
            dbContext.AppointmentSchedules.Remove(schedule);
            await dbContext.SaveChangesAsync();
            dbContext.AppointmentSchedules.Add(newSchedule);
			await dbContext.SaveChangesAsync();
		}
	}
}
