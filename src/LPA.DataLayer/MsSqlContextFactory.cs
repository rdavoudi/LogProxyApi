using LPA.DataLayer.Context;
using LPA.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LPA.DataLayer
{
    public class MsSqlContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Using `{basePath}` as the ContentRootPath");
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(basePath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build();

            services.AddSingleton<IConfigurationRoot>(provider => configuration);
            services.Configure<ApiSettings>(options => configuration.Bind(options));

            var apiSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<ApiSettings>>();

            //services.AddEntityFrameworkSqlServer();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(apiSettings.Value.ConnectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
