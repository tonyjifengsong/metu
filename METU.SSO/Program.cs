using METU.CACHES;
using METU.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace METU.SSO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string url = ConfigHelper.GetConfigSettings("configurl");// "http://localhost:2222";//"http://47.114.146.15:2222";
            CommonCache.ConfigURL = url;
            CommonCache.LogURL = ConfigHelper.GetConfigSettings("ElasticSearchContext:esurl");
            List<string> lst = new List<string>();
            lst.Add(CommonCache.LogURL);
            CommonCache.ConfigCaches.Add("ESCONFIG", lst);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
