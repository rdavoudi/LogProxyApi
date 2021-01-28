using LPA.DataLayer.Context;
using LPA.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DataLayer
{
    public static class DBServiceExtensions
    {
        public static IServiceCollection AddConfiguredMsSqlDbContext(this IServiceCollection services, ApiSettings apiSettings)
        {
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
           
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(apiSettings.ConnectionString,
                 sqlServerOptionsBuilder =>
                 {
                     sqlServerOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                     sqlServerOptionsBuilder.EnableRetryOnFailure();
                     sqlServerOptionsBuilder.MigrationsAssembly(typeof(DBServiceExtensions).Assembly.FullName);
                 }));

            return services;
        }
    }
}
