using DataModels;
using Microsoft.EntityFrameworkCore;

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

		public async Task UpdateAsync(AppointmentSchedule schedule, DateTime sTime, DateTime eTime)
		{
			var newSchedule = new AppointmentSchedule()
			{
				StartTime = sTime,
				EndTime = eTime,
				DentistId = schedule.DentistId,
				CustomerId = schedule.CustomerId
			};
            dbContext.AppointmentSchedules.Remove(schedule);
            await dbContext.SaveChangesAsync();
            dbContext.AppointmentSchedules.Add(newSchedule);
			await dbContext.SaveChangesAsync();
		}
	}
}
