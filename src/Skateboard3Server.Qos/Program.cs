using System.Net;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Skateboard3Server.Qos.Controllers;

namespace Skateboard3Server.Qos;

public class Program
{
    public static void Main(string[] args)
    {
        //Setup NLog
        LogManager.Setup().LoadConfigurationFromAppSettings();
        QosController.PublicIp = new HttpClient().GetStringAsync("https://checkip.amazonaws.com/").GetAwaiter().GetResult().Trim();
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            })
            .UseNLog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        //qos servers [gosgvaprod-qos01, gosiadprod-qos01, gossjcprod-qos01] (HTTP)
                        serverOptions.Listen(IPAddress.Any, 17502);
                    })
                    .UseStartup<Startup>();
            });
    }
}