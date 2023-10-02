
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.Error;
using TakeMe.InferStructuer;
using TakeMe.InferStructuer.Data;
using TakeMe.InferStructuer.Repositries;
using TakeMe.Middlware;

namespace TakeMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(op =>
            {
                op.InvalidModelStateResponseFactory = context =>
                {
                    var errorRespone = new APIValidationError
                    {
                        Error = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray(),
                    };
                    return new BadRequestObjectResult(errorRespone);
                };
            });
            ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdentity<AppUsers, IdentityRole>
                ().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.infarStrctureregistration(builder.Configuration);

            builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            ///

            builder.Services.AddSwaggerGen(op =>
            {
                var securty = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "jwt Auth Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                op.AddSecurityDefinition("Bearer", securty);
                var SR = new OpenApiSecurityRequirement { { securty, new[] { "Bearer" } } };
                op.AddSecurityRequirement(SR);
            });







            //Add Authentication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                // x.RequireHttpsMetadata = false;
                // x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ve...@!.#ryv.][erysecret...@!.#2.][pws@]")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                };
            });
            builder.Services.AddScoped<ITokenService, TokenService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddliWare>();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors(op => op.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());



            app.MapControllers();

            app.Run();
        }
    }
}