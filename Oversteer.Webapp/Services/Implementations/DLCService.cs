using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;
using System.Net.Http;

namespace Oversteer.Webapp.Services
{
    public class DLCService : IDLCService
    {
        private readonly ApplicationDbContext _db;

        public DLCService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<List<Dlc>> GetDLCs()
        {
            var dlcs = _db.Dlcs.Include(d => d.RaceSim).OrderBy(d => d.Name).ToList();
            return Task.FromResult(dlcs);
        }

        public Task RemoveDlc(Dlc dlc)
        {
            if (_db.Dlcs.Any(c => c.Id == dlc.Id))
            {
                _db.Dlcs.Remove(dlc);
                _db.SaveChanges();
            }

            return Task.CompletedTask;
        }

        public async Task UpsertDlc(Dlc dlc)
        {
            if (!_db.Dlcs.Any(c => c.Id == dlc.Id))
            {
                await _db.Dlcs.AddAsync(dlc);
            }
            else
            {
                _db.Entry(dlc).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }
    }
}
