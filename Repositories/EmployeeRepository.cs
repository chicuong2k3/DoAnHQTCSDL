using DataModels;
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
    }
}
