using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace LPA.MsTests
{
    public static class TestsHttpClient
    {
        private static readonly Lazy<HttpClient> _serviceProviderBuilder =
                new Lazy<HttpClient>(getHttpClient, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// A lazy loaded thread-safe singleton
        /// </summary>
        public static HttpClient Instance { get; } = _serviceProviderBuilder.Value;

        private static HttpClient getHttpClient()
        {
            var services = new CustomWebApplicationFactory();
            //var services = new WebApplicationFactory<Program>();

            return services.CreateClient(); //NOTE: This action is very time consuming, so it should be defined as a singleton.
        }
    }
}
