using DataModels;
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
    }
}
