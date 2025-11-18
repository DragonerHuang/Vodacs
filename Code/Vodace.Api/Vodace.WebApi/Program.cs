using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Api.Controllers.MqDataHandle;

namespace Vodace.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.ConfigureKestrel(serverOptions =>
                       {
                           serverOptions.Limits.MaxRequestBodySize = 10485760;
                           // Set properties and call methods on options
                       });
                       webBuilder.UseKestrel().UseUrls("http://*:9991");
                       webBuilder.UseIIS();
                       webBuilder.UseStartup<Startup>();
                   }).UseServiceProviderFactory(new AutofacServiceProviderFactory());

    }
}
