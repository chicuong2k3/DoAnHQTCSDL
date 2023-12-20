using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CustomerRepository
    {
        private readonly AppDbContext dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            dbContext.Customers.Update(customer);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Customer?> GetCustomerByAccountAsync(AppUser user)
		{

            return await dbContext.Customers.Where(x => x.AccountId == user.Id).SingleOrDefaultAsync();
		}

		public async Task<Customer?> GetCustomerByIdAsync(string id)
		{

			return await dbContext.Customers.FindAsync(id);
		}
	}
}
