using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace Oversteer.Scheduler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();
                    string mySqlConnstring = builtConfig["ConnectionStrings:DefaultConnection"];

                    GlobalConfiguration.Configuration.UseStorage(
                        new MySqlStorage(
                            mySqlConnstring,
                            new MySqlStorageOptions
                            {
                                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                                QueuePollInterval = TimeSpan.FromSeconds(15),
                                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                PrepareSchemaIfNecessary = true,
                                DashboardJobListLimit = 50000,
                                TransactionTimeout = TimeSpan.FromMinutes(1),
                                TablesPrefix = "Hangfire"
                            }));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WorkerService>();
                    services.AddHangfire(configuration => configuration
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings());
                    services.AddHangfireServer(options =>
                    {
                        options.Queues = new[] { "default" };
                    });

                });
    }
}
