using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;

namespace Oversteer.Webapp.Services
{
    public class RaceSimService : IRaceSimService
    {
        private readonly ApplicationDbContext _db;

        public RaceSimService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Models.RaceSim>> GetAllRaceSims()
        {
            return _db.RaceSims.OrderBy(c => c.Name).ToList();
        }

        public async Task RemoveSim(Models.RaceSim raceSim)
        {
            if (await _db.RaceSims.AnyAsync(c => c.Id == raceSim.Id))
            {
                _db.RaceSims.Remove(raceSim);
                _db.SaveChanges();
            }
        }

        public async Task UpsertRaceSim(RaceSim raceSim)
        {
            if (_db.RaceSims.Any(r => r.Id == raceSim.Id))
            {
                await _db.RaceSims.AddAsync(raceSim);
            }
            else
            {
                _db.Entry(raceSim).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }

        public async Task SaveCarInSim(CarInRaceSim carInRaceSim)
        {
            if (!await _db.CarInRaceSims.AnyAsync(c => c.CarId == carInRaceSim.CarId && c.RaceSimId == carInRaceSim.RaceSimId))
            {
                await _db.CarInRaceSims.AddAsync(carInRaceSim);
            }
            else
            {
                _db.Entry(carInRaceSim).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();

        }

        public List<CarInRaceSim> GetAllCarsInRaceSim(Guid carid)
        {
            return _db.CarInRaceSims.Include(c => c.Car).Include(c => c.RaceSim).Where(c => c.CarId == carid).ToList();
        }

        public async Task SaveTrackInSim(TrackInRaceSim trackInRaceSim)
        {
            if (!await _db.TrackInRaceSims.AnyAsync(c => c.TrackId == trackInRaceSim.TrackId && c.RaceSimId == trackInRaceSim.RaceSimId))
            {
                await _db.TrackInRaceSims.AddAsync(trackInRaceSim);
            }
            else
            {
                _db.Entry(trackInRaceSim).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }

        public List<TrackInRaceSim> GetAllTracksInRaceSim(Guid trackid)
        {
            return _db.TrackInRaceSims.Include(c => c.Track).Include(c => c.RaceSim).Where(c => c.TrackId == trackid).ToList();
        }
    }
}
