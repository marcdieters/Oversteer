using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages
{
    public partial class Index
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private DateTime UserDate { get; set; }
        private DateTime UTCDate { get; set; }
        private DateTime ServerDate { get; set; }
        private int TimeZoneOffset { get; set; }

        protected async override Task OnInitializedAsync()
        {

        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                this.ServerDate = DateTime.Now;
                this.UserDate = await JSRuntime.InvokeAsync<DateTime>("localDate");
                this.UTCDate = await JSRuntime.InvokeAsync<DateTime>("utcDate");
                this.TimeZoneOffset = await JSRuntime.InvokeAsync<int>("timeZoneOffset");
            }
        }
    }
}
