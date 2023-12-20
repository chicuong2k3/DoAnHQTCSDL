using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class PersonalScheduleRepository
    {
        private readonly AppDbContext dbContext;

        public PersonalScheduleRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAppoinment(PersonalSchedule schedule)
        {
            await dbContext.PersonalSchedules.AddAsync(schedule);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<PersonalSchedule>> GetAllSchedulesBelongToADentist(string dentistId)
        {
            return await dbContext.PersonalSchedules
                .Where(x => x.DentistId == dentistId).ToListAsync();
        }
    }
}
