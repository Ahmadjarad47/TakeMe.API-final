using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;

namespace TakeMe.InferStructuer.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<AppUsers> AppUsers { get; set; }
        public virtual DbSet<RegisterDaily> RegisterDailies { get; set; }
        public virtual DbSet<QRCodeCheck> QRCodeChecks { get; set; }
        public virtual DbSet<Postes> Postes { get; set; }
        public virtual DbSet<Copone> Copones { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
      
    }
}
