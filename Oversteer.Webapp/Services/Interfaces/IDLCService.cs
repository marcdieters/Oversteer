using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface IDLCService
    {
        Task<List<Models.Dlc>> GetDLCs();
        Task UpsertDlc(Models.Dlc dlc);
        Task RemoveDlc(Models.Dlc dlc);
    }
}
