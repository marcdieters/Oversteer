using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models;
using Oversteer.Server.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Oversteer.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configJson = File.ReadAllText("appsettings.json");
            var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(configJson);

            Globals.ApiBaseUri = config["OversteerBaseUri"].ToString();

            AppDbContext db = new AppDbContext();

            if (!db.Hosts.Any())
            {
                Console.WriteLine("Enter the authentication key you see on the add host screen");
                string enteredKey = Console.ReadLine();

                try
                {
                    var token = Token.Generate("registerhost", new List<string>());
                    string userId = await Api.Post(Globals.ApiBaseUri, "api/host/verify", enteredKey, token);

                    Models.Host host = new Models.Host();
                    host.Name = Environment.MachineName;
                    host.Ping = DateTime.Now;
                    host.QueueName = Guid.NewGuid().ToString();
                    host.ApplicationUserId = Guid.Parse(userId);
                    string secret = Cryptography.GenerateRandomPassword();
                    host.Secret = Cryptography.EncryptString(secret);

                    db.Hosts.Add(host);
                    db.SaveChanges();

                    await Api.Post(Globals.ApiBaseUri, "api/host", host, token);

                    Console.WriteLine("Host registration is complete but it's not ready for placement yet. Go to the hosts page to complete the configuration. Press enter to continue.");
                    Console.ReadLine();
                }
                catch
                {
                    Console.WriteLine("Host registration failed. You cannot continue. Press enter to exit");
                    Console.ReadLine();
                    return;
                }
            }

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            Globals.QueueName = db.Hosts.First().QueueName;

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WorkerService>();
                });
    }
}
