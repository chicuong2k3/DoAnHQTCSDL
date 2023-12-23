using DataModels;
using DataModels.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MedicineMedicalRecordRespository
    {
        private AppDbContext dbContext;
        private DapperContext dapperContext;
        public MedicineMedicalRecordRespository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Add()
        {
            //use proc here

        }
    }
}
