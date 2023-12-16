using DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Repositories
{
    public class EmployeeRepository
    {
        private readonly AppDbContext dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddEmployeeAsync(Employee employee)
        {
            await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();
        }
		public async Task<Employee> GetEmployeeByAccountAsync(AppUser user)
		{
			return await dbContext.Employees.Where(x => x.AccountId == user.Id).SingleOrDefaultAsync();
		}
	}
}
