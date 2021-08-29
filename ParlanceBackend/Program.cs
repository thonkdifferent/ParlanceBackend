using Microsoft.AspNetCore.Hosting;
using ParlanceBackend.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ParlanceBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }
        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ProjectContext>();

            var migrationSuccess = false;
            while (!migrationSuccess)
            {
                try
                {
                    //Apply required migrations
                    context.Database.Migrate();
                    migrationSuccess = true;
                }
                catch (NpgsqlException ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    if (ex.InnerException is SocketException {SocketErrorCode: SocketError.ConnectionRefused})
                    {
                        logger.LogError(ex, "Can't connect to the database. Retrying in 5 seconds.");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(options =>
                {
                    options.AddEnvironmentVariables("PARLANCE_");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
