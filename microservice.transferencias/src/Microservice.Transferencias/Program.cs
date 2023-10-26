
using Steeltoe.Management.Endpoint;
using WyD.Mess.Hosting.WindowsServices;
using WyD.Mess.Logging;

namespace Microservice.Transferencias
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                })
                .ConfigureAppConfiguration((host, config) =>
                {
                    //config.AddJsonFile("datasources.json", optional: false);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>();
                })
                //.UseLogging()
                .AddHealthActuator()
                .AddInfoActuator()
                .AddLoggersActuator()
                .UseService();
    }
}