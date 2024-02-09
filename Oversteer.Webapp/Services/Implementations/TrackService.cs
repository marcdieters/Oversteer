using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;

namespace Oversteer.Webapp.Services
{
    public class TrackService : ITrackService
    {
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _db;

        public TrackService(IImageService imageService, ApplicationDbContext db)
        {
            _imageService = imageService;
            _db = db;
        }

        public Task<List<Track>> GetTracks()
        {
            var tracks = _db.Tracks.Include(c => c.Country)
                .Include(c => c.TrackLayouts)
                .Include(c => c.TrackInRaceSims)
                .Include(c => c.Country)
                .OrderBy(c => c.Name).ToList();

            return Task.FromResult(tracks);
        }

        public async Task<Guid> UpsertTrack(Track track, byte[] fileContent)
        {
            await _imageService.SaveImage(fileContent, Path.Combine("img", track.SceneryImage));

            if (!_db.Tracks.Any(c => c.Id == track.Id))
            {
                _db.Tracks.Add(track);
            }
            else
            {
                _db.Entry(track).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return track.Id;
        }

        public async Task RemoveTrack(Track track)
        {
            if (_db.Tracks.Any(c => c.Id == track.Id))
            {
                _db.Tracks.Remove(track);
                _db.SaveChanges();

                await _imageService.RemoveImage("img", track.SceneryImage);
            }
        }

        public Task<List<TrackLayout>> GetTrackLayouts(Guid trackId)
        {
            var layouts = _db.TrackLayouts.Where(t => t.TrackId == trackId).OrderBy(t => t.Name).ToList();
            return Task.FromResult(layouts);
        }

        public Task UpsertTrackLayout(TrackLayout layout)
        {
            if (layout.Id == Guid.Empty)
            {
                _db.TrackLayouts.Add(layout);
            }
            else
            {
                _db.Entry(layout).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task RemoveTrackLayout(TrackLayout layout)
        {
            if (_db.TrackLayouts.Any(c => c.Id == layout.Id))
            {
                _db.TrackLayouts.Remove(layout);
                _db.SaveChanges();

                await _imageService.RemoveImage("img", layout.LayoutImage);
            }
        }
    }
}
