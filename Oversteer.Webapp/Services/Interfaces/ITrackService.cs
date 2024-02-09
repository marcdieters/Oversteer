using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface ITrackService
    {
        Task<List<Track>> GetTracks();
        Task<Guid> UpsertTrack(Track track, byte[] fileContent);
        Task RemoveTrack(Track track);

        Task<List<TrackLayout>> GetTrackLayouts(Guid trackId);
        Task UpsertTrackLayout(TrackLayout layout);
        Task RemoveTrackLayout(TrackLayout layout);
    }
}
