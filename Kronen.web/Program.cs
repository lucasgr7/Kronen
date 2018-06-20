using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kronen
{
    public class Program
    {

        private static string port = "5000";
        public static void Main(string[] args)
        {
            
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
            // BuildWebHost(args).Run();
        }

        // public static IWebHost BuildWebHost(string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseStartup<Startup>()
        //         .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"))
        //         .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
        //         .Build();
    }
}
