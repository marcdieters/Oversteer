using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface IRaceSimService
    {
        Task<List<Models.RaceSim>> GetAllRaceSims();
        Task UpsertRaceSim(Models.RaceSim raceSim);
        Task RemoveSim(Models.RaceSim raceSim);
        
        Task SaveCarInSim(CarInRaceSim carInRaceSim);
        List<CarInRaceSim> GetAllCarsInRaceSim(Guid carid);

        Task SaveTrackInSim(TrackInRaceSim trackInRaceSim);
        List<TrackInRaceSim> GetAllTracksInRaceSim(Guid trackid);
    }
}
