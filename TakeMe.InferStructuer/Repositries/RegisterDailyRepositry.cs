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
    public class RegisterDailyRepositry : GenericRepositrie<RegisterDaily>, IRegisterDaily
    {
        private readonly ApplicationDbContext context;

        public RegisterDailyRepositry(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<bool> IsAreadyExist(string UserId)
        {
            var check = await context.RegisterDailies
                  .AsNoTracking().AnyAsync(e => e.AppUserId.Equals(UserId));
            return check;
        }
    }
}
