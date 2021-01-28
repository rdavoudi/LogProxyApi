using LPA.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.Ioc
{
    public static class CustomServicesRegistry
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();

            return services;
        }
    }
}
