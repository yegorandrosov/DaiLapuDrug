using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace DaiLapuDrug.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseContentRoot(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
                    //    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) : Directory.GetCurrentDirectory());
                });
    }
}
