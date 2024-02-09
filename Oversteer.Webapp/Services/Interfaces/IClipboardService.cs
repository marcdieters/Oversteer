using System.Threading.Tasks;

namespace Oversteer.Webapp.Services
{
    public interface IClipboardService
    {
        Task<string> ReadFromClipboard();
        Task WriteToClipboard(string text);
    }
}
