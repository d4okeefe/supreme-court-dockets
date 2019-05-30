using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SupremeCourtDocketApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();





            //attempt to copy from simpledockets.db to dockets.db
            // using (var scope = host.Services.CreateScope())
            // {
            //     var services = scope.ServiceProvider;

            //     try
            //     {
            //         //var context_simple = services
            //         //    .GetRequiredService<Models.SupremeCourtDocketSimpleAppContext>();
            //         //context_simple.Database.Migrate();

            //         //var context = services
            //         //    .GetRequiredService<Models.SupremeCourtDocketAppContext>();
            //         //context.Database.Migrate();

            //         Models.CaptureDocketsFromDb.SeedDb.TransferData(services);
            //     }
            //     catch (Exception ex)
            //     {
            //         var logger = services.GetRequiredService<ILogger<Program>>();
            //         logger.LogError(ex, "An error occurred seeding the DB.");
            //     }
            // }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
