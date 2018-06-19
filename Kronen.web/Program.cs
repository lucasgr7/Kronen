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
            if(Environment.GetEnvironmentVariable("PORT") != null){
                port = Environment.GetEnvironmentVariable("PORT").ToString();
            }
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:" + port)
                .Build();
    }
}
