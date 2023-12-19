using DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class MedicineInventoryRepository
	{
		private AppDbContext dbContext;
		public MedicineInventoryRepository (AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		
		public async Task<List<MedicineInventory>> GetAllMedicineInventory ()
		{
			var data = await dbContext.MedicineInventories.ToListAsync();
			return data;
		}

		public async Task InsertMedicineInvetory(MedicineInventory model)
		{
			await dbContext.MedicineInventories.AddAsync(model);
			await dbContext.SaveChangesAsync();
		}

		public async Task Delete (int id)
		{
			var item = await dbContext.MedicineInventories.FindAsync(id);
			if(item != null)
			{
				dbContext.MedicineInventories.Remove(item);
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task Update(MedicineInventory model)
		{
			var item = await dbContext.MedicineInventories.FindAsync(model.Id);
			if(item != null)
			{
				dbContext.Entry(item).CurrentValues.SetValues(model);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
