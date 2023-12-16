using DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Repositories
{
    public class DentistRepository
    {
        private readonly AppDbContext dbContext;

        public DentistRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddDentistAsync(Dentist dentist)
        {
            await dbContext.Dentists.AddAsync(dentist);
            await dbContext.SaveChangesAsync();
        }
		public async Task<Dentist> GetDentistByAccountAsync(AppUser user)
		{
			return await dbContext.Dentists.Where(x => x.AccountId == user.Id).SingleOrDefaultAsync();
		}
	}
}
