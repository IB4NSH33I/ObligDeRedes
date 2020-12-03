using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ServerInstaPhoto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread serverThread = new Thread(()=> InitConnection());
            serverThread.Start();
            CreateHostBuilder(args).Build().Run();
        }

        public static void InitConnection()
        {
            Server server = new Server();
            server.SearchActiveConnections();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
