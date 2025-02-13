using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.Exporter.Tracing;
using Steeltoe.Management.Tracing;
using System;

namespace METU.Eureka
{
    public static partial class ServiceCollectionExtensions
    {
        #region added by tony 2019-12-31
        /*  /// "eureka": {
          "client": {
             "serviceUrl": "http://localhost:8000/eureka/",
             "shouldFetchRegistry": true,
             "validateCertificates": false
           },
           "instance": {
             "port": 8001,
             "preferIpAddress": true,
             "instanceId": "net-service-service-container:8001"
           }
         }*/
        /// <summary>
        /// Eureka
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddEureka(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddDiscoveryClient(Configuration);
        }
        /// <summary>
        /// AddHystrixMetrics
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddHystrixMetrics(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddSingleton<IClientService, ClientService>();
            // Add Steeltoe Discovery Client service
            services.AddDiscoveryClient(Configuration);
            // Add Steeltoe handler to container
            services.AddTransient<DiscoveryHttpMessageHandler>();
            // Configure a HttpClient
            services.AddHttpClient("client-api-values", c =>
            {
                c.BaseAddress = new Uri(Configuration["Services:Client-Service:Url"]);
            })
            .AddHttpMessageHandler<DiscoveryHttpMessageHandler>()
            .AddTypedClient<IClientService, ClientService>();
            // Add Steeltoe Hystrix Command
            services.AddHystrixCommand<ClientServiceCommand>("ClientService", Configuration);
            // Add Steeltoe Distributed Tracing
            services.AddDistributedTracing();
            // Export traces to Zipkin
           services.AddZipkinExporter(Configuration);

           // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add Hystrix Metrics to container
          services.AddHystrixMetricsStream(Configuration);
        }
        #endregion

    }
}
