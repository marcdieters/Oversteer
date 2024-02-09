using Oversteer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Oversteer.Models
{
    public class ACCServer
    {
        public ACCServer()
        {
            ACCSettings = new ACCSettings();
            ACCEvent = new ACCEvent();
            ACCConfiguation = new ACCConfiguation();
            ACCAssistRules = new ACCAssistRules();
            ACCEventRules = new ACCEventRules();
            ACCEntry = new ACCEntry();
        }

        public ACCSettings ACCSettings { get; set; }
        public ACCEvent ACCEvent { get; set; }
        public ACCConfiguation ACCConfiguation { get; set; }
        public ACCAssistRules ACCAssistRules { get; set; }
        public ACCEventRules ACCEventRules { get; set; }
        public ACCEntry ACCEntry { get; set; }
    }

    public class ACCSettings
    {
        public ACCSettings()
        {
            adminPassword = "";
            adminPassword = "";
            spectatorPassword = "";
            centralEntryListPath = "";
            maxCarSlots = 64;
            configVersion = 1;
            dumpLeaderboards = 1;
            shortFormationLapBool = true;
        }

        public string serverName { get; set; }
        public string adminPassword { get; set; }
        public string carGroup { get; set; }
        public int trackMedalsRequirement { get; set; }
        public int safetyRatingRequirement { get; set; }
        public int racecraftRatingRequirement { get; set; }
        public string password { get; set; }
        public int maxCarSlots { get; set; }
        public string spectatorPassword { get; set; }
        public int configVersion { get; set; }
        public int shortFormationLap { get; set; }
        public int allowAutoDQ { get; set; }
        public string centralEntryListPath { get; set; }
        public int dumpLeaderboards { get; set; }
        public int formationLapType { get; set; }
        [JsonIgnore]
        public bool shortFormationLapBool { get; set; }
        [JsonIgnore]
        public CarGroup carGroupEnum { get; set; }
    }

    public class ACCEvent
    {
        public string track { get; set; }
        public int preRaceWaitingTimeSeconds { get; set; }
        public int sessionOverTimeSeconds { get; set; } = 60;
        public int ambientTemp { get; set; } = 19;
        public double cloudLevel { get; set; } = 0.0;
        public double rain { get; set; } = 0.0;
        public int weatherRandomness { get; set; } = 0;
        public int postQualySeconds { get; set; } = 60;
        public int postRaceSeconds { get; set; } = 60;
        public int isFixedConditionQualification { get; set; }
        public List<ACCSession> sessions { get; set; } = new List<ACCSession>();
        public int configVersion { get; set; } = 1;
    }

    public class ACCSession
    {
        public ACCSession()
        {
            timeMultiplier = 1;
        }


        public int hourOfDay { get; set; }
        public int dayOfWeekend { get; set; }
        public int timeMultiplier { get; set; }
        public string sessionType { get; set; }
        public int sessionDurationMinutes { get; set; }
    }

    public class ACCConfiguation
    {
        public int udpPort { get; set; }
        public int tcpPort { get; set; }
        public int maxConnections { get; set; } = 64;
        public int configVersion { get; set; } = 1;
    }

    public class ACCAssistRules
    {
        public ACCAssistRules()
        {
            disableIdealLine = 1;
            disableAutosteer = 1;
            disableAutosteer = 0;
            disableAutoPitLimiter = 1;
            disableAutoGear = 1;
            disableAutoEngineStart = 0;
            disableAutoWiper = 0;
            disableAutoLights = 0;
        }

        public int disableIdealLine { get; set; }
        public int disableAutosteer { get; set; }
        public int stabilityControlLevelMax { get; set; }
        public int disableAutoPitLimiter { get; set; }
        public int disableAutoGear { get; set; }
        public int disableAutoClutch { get; set; }
        public int disableAutoEngineStart { get; set; }
        public int disableAutoWiper { get; set; }
        public int disableAutoLights { get; set; }
    }

    public class ACCEventRules
    {
        public ACCEventRules()
        {
            qualifyStandingType = 1;
            pitWindowLengthSec = -1;
            driverStintTimeSec = -1;
            mandatoryPitstopCount = 0;
            maxTotalDrivingTime = -1;
            maxDriversCount = 0;
            tyreSetCount = 50;
            isRefuellingTimeFixed = false;
            isMandatoryPitstopSwapDriverRequired = false;
        }

        public int qualifyStandingType { get; set; }
        public int pitWindowLengthSec { get; set; }
        public int driverStintTimeSec { get; set; }
        public int mandatoryPitstopCount { get; set; }
        public int maxTotalDrivingTime { get; set; }
        public int maxDriversCount { get; set; }
        public bool isRefuellingAllowedInRace { get; set; }
        public int tyreSetCount { get; set; }
        public bool isRefuellingTimeFixed { get; set; }
        public bool isMandatoryPitstopRefuellingRequired { get; set; }
        public bool isMandatoryPitstopTyreChangeRequired { get; set; }
        public bool isMandatoryPitstopSwapDriverRequired { get; set; }
    }

    public class ACCEntry
    {
        public ACCEntryList[] entries { get; set; }
        public int forceEntryList { get; set; }
    }

    public class ACCEntryList
    {
        public ACCEntryList()
        {
            overrideDriverInfo = 1;
            isServerAdmin = 0;
            defaultGridPosition = 0;
            ballastKg = 0;
            restrictor = 0;
            customCar = "";
        }

        public ACCDriver[] drivers { get; set; }
        public int raceNumber { get; set; }
        public int forcedCarModel { get; set; }
        public int overrideDriverInfo { get; set; }
        public int isServerAdmin { get; set; }
        public string customCar { get; set; }
        public int defaultGridPosition { get; set; }
        public int ballastKg { get; set; }
        public int restrictor { get; set; }
    }

    public class ACCDriver
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string shortName { get; set; }
        public int driverCategory { get; set; }
        public string playerID { get; set; }
    }

    public enum CarGroup
    {
        [Display(Name = "Free for all")]
        FreeForAll,
        GT3,
        GT4,
        [Display(Name = "Porsche Cup")]
        Cup,
        [Display(Name = "Lamborghini Supertrofeo")]
        ST
    }
}
