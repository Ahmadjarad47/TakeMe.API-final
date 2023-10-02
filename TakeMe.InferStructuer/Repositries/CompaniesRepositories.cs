using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.InferStructuer.Data;

namespace TakeMe.InferStructuer.Repositries
{
    public class CompaniesRepositories : GenericRepositrie<Company>, ICompanies
    {
        private readonly ApplicationDbContext context;

       
        public CompaniesRepositories(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> CheckIshere(string name)
        {
           var check=await context.Companies.AsNoTracking().AnyAsync(n=>n.Name==name);
            return check;
        }

        public async Task<List<Postes>> getId(int name)
        {
            var post = await context.Postes.AsNoTracking().ToListAsync();
            List<Postes> check = new();
            foreach (var item in post)
            {
               check=await context.Postes.Where(n=>n.companyId==name).ToListAsync();
            }
          
            return check;
        }

        public async Task<Company> getName(string name)
        {
            var result = await context.Companies.AsNoTracking().FirstOrDefaultAsync(n=>n.Name==name);
            if (result is null)
            {
                return null;
            }
            return result;
        }
    }
}
