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
    }
}
