using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SurruhBackend.Models;

namespace SurruhBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var buildWebHost = BuildWebHost(args);

            using (var scope = buildWebHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogDebug((services != null).ToString());

                // Set up DB seed
                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }


            }

            buildWebHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
