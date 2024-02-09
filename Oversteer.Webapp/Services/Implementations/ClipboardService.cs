using Microsoft.JSInterop;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Oversteer.Webapp.Services
{
    public class ClipboardService : IClipboardService
    {
        private readonly IJSRuntime _jsRuntime;

        public ClipboardService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> ReadFromClipboard()
        {
            return await _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
        }

        public async Task WriteToClipboard(string text)
        {
            await _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
    }
}
