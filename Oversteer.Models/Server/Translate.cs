using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class Translate
    {
        public static ACCServer AccObjectToAccServer(Server server)
        {
            ACCServer accServer = new ACCServer();

            accServer.ACCSettings.serverName = server.Name;
            accServer.ACCSettings.adminPassword = server.AdminPassword;
            accServer.ACCSettings.maxCarSlots = 64;
            accServer.ACCSettings.allowAutoDQ = 0;
            accServer.ACCSettings.carGroup = server.CarClass.Name;
            accServer.ACCSettings.dumpLeaderboards = 1;
            accServer.ACCSettings.password = server.Password;
            accServer.ACCSettings.racecraftRatingRequirement = server.ACC.RaceCraftRating;
            accServer.ACCSettings.safetyRatingRequirement = server.ACC.SafetyRating;
            accServer.ACCSettings.trackMedalsRequirement = server.ACC.TrackRating;
            accServer.ACCSettings.formationLapType = server.ACC.FormationLapType;
            accServer.ACCSettings.shortFormationLap = Convert.ToInt32(server.ACC.ShortFormationLap);

            accServer.ACCAssistRules.disableAutoClutch = Convert.ToInt32(server.ACC.DisableAutoClutch);
            accServer.ACCAssistRules.disableAutoEngineStart = Convert.ToInt32(server.ACC.DisableAutoEngineStart);
            accServer.ACCAssistRules.disableAutoGear = Convert.ToInt32(server.ACC.DisableAutoGear);
            accServer.ACCAssistRules.disableAutoLights = Convert.ToInt32(server.ACC.DisableAutoLight);
            accServer.ACCAssistRules.disableAutoPitLimiter = Convert.ToInt32(server.ACC.DisableAutoPitLimiter);
            accServer.ACCAssistRules.disableAutosteer = Convert.ToInt32(server.ACC.DisableAutoSteer);
            accServer.ACCAssistRules.disableAutoWiper = Convert.ToInt32(server.ACC.DisableAutoWiper);
            accServer.ACCAssistRules.stabilityControlLevelMax = Convert.ToInt32(server.ACC.StabilityControlLevelMax);

            accServer.ACCConfiguation.tcpPort = server.TCPPort;
            accServer.ACCConfiguation.udpPort = server.UDPPort;

            accServer.ACCEvent.ambientTemp = server.ACC.AmbientTemp;
            accServer.ACCEvent.cloudLevel = server.ACC.CloudLevel;
            accServer.ACCEvent.rain = server.ACC.Rain;
            accServer.ACCEvent.track = server.Track.TrackInRaceSims.First(t => t.RaceSimId == server.RaceSimId).NameInGame;

            foreach(var session in server.Sessions)
            {
                ACCSession accSession = new ACCSession();
                accSession.timeMultiplier = session.TimeMultiplier;
                accSession.dayOfWeekend = Convert.ToInt32(session.DayOfTheWeek);
                accSession.hourOfDay = session.Hour;
                accSession.sessionDurationMinutes = session.DurationMinutes;

                switch (session.SessionType)
                {
                    case Enums.SessionType.Practice:
                        accSession.sessionType = "P";
                        break;

                    case Enums.SessionType.Qualifying:
                        accSession.sessionType = "Q";
                        break;

                    case Enums.SessionType.Race:
                        accSession.sessionType = "R";
                        break;
                }

                accServer.ACCEvent.sessions.Add(accSession);
            }

            return accServer;
        }
    }
}
