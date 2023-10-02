using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TakeMe.Core.Interfaces;
using TakeMe.InferStructuer.Data;
using TakeMe.InferStructuer.Repositries;

namespace TakeMe.InferStructuer
{
    public static class InfarStrctureRegistration
    {

        public static IServiceCollection infarStrctureregistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepositrie<>), typeof(GenericRepositrie<>));

            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            },ServiceLifetime.Transient);
           services.AddSingleton<IEmailService, EmailService>();
            services.AddMemoryCache();
            services.AddScoped<ITokenService,TokenService>();

            return services;
        }
       
    }
}
