using LPA.DataLayer;
using LPA.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.Ioc
{
    public static class DBServicesRegistry
    {
        public static void AddDBServices(this IServiceCollection services)
        {
            var apiSettings = GetApiSettings(services);
            services.AddConfiguredMsSqlDbContext(apiSettings);
        }

        public static ApiSettings GetApiSettings(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var apiSettingsOptions = provider.GetRequiredService<IOptionsSnapshot<ApiSettings>>();
            var apiSettings = apiSettingsOptions.Value;
            if (apiSettings == null) throw new ArgumentNullException(nameof(apiSettings));
            return apiSettings;
        }
    }
}
