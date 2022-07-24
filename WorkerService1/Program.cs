using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using WorkerService1.Services;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"D:\Workspace\temp_log\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");
                IHost host = CreateHostBuilder(args).Build();
                CreateDatabaseIfNotExist(host);
                host.Run();
                return;

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }


            //CreateHostBuilder(args).Build().Run();
        }

        private static void CreateDatabaseIfNotExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.EnsureCreated();
                }
                catch(Exception)
                {
                    throw;
                }

            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    AppSettings.Configuration = configuration;
                    AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConn");

                    var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
                    optionBuilder.UseSqlServer(AppSettings.ConnectionString);

                    services.AddScoped<AppDbContext>(d => new AppDbContext(optionBuilder.Options));

                    services.AddHostedService<Worker>();
                })
                .UseSerilog();

    }
}
