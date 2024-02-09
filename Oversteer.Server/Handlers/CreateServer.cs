using Oversteer.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oversteer.Models;
using Oversteer.Server.Data;

namespace Oversteer.Server
{
    public  class Server
    {
        public async static void Create(string body)
        {
            AppDbContext db = new AppDbContext();
            var host = db.Hosts.First();

            HostPreAuthDto hostPreAuthDto = new HostPreAuthDto();
            hostPreAuthDto.HostSecret = host.Secret;
            hostPreAuthDto.HostId = host.Id;

            // Authorize host
            var response = await Api.Post(Globals.ApiBaseUri, "api/host/preauth", hostPreAuthDto, string.Empty);
            HostPreAuthResponse hostPreAuthResponse = JsonConvert.DeserializeObject<HostPreAuthResponse>(response);

            HostLogin hostLogin = new HostLogin();
            hostLogin.HostId = host.Id;
            hostLogin.HostSecret = host.Secret;
            hostLogin.Token = hostPreAuthResponse.Token;
            string token = await Api.Post(Globals.ApiBaseUri, "api/account/devicelogin", hostLogin, string.Empty);

            // Create server
            Models.Server server = JsonConvert.DeserializeObject<Models.Server>(body);

            byte[] bytes = await Api.GetFile(Globals.ApiBaseUri, $"api/server/download/{server.RaceSimId}", token);

            string serverName = Guid.NewGuid().ToString();
            string serverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, serverName);
            string serverFile = Path.Combine(serverPath, "server.zip");

            Directory.CreateDirectory(serverPath);
            File.WriteAllBytes(serverFile, bytes);
            ZipFile.ExtractToDirectory(serverFile, serverPath);
            File.Delete(serverFile);

            string confileFilePath = Path.Combine(serverPath, "cfg");
            ACCServer aCCServer = Translate.AccObjectToAccServer(server);

            string json = JsonConvert.SerializeObject(aCCServer.ACCAssistRules);
            File.WriteAllText(Path.Combine(confileFilePath, "assistRules.json"), json);

            json = JsonConvert.SerializeObject(aCCServer.ACCConfiguation);
            File.WriteAllText(Path.Combine(confileFilePath, "configuration.json"), json);

            json = JsonConvert.SerializeObject(aCCServer.ACCEvent);
            File.WriteAllText(Path.Combine(confileFilePath, "event.json"), json);

            json = JsonConvert.SerializeObject(aCCServer.ACCSettings);
            File.WriteAllText(Path.Combine(confileFilePath, "settings.json"), json);

            json = JsonConvert.SerializeObject(aCCServer.ACCEntry);
            File.WriteAllText(Path.Combine(confileFilePath, "entrylist.json"), json);

            json = JsonConvert.SerializeObject(aCCServer.ACCEventRules);
            File.WriteAllText(Path.Combine(confileFilePath, "eventRules.json"), json);
        }
    }
}
