using Microsoft.AspNetCore.Components;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Server
{
    public partial class _UpsertServer
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICountryService CountryService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }
        [Inject]
        protected IAccountService AccountService { get; set; }
        protected IServerService ServerService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "serverid")]
        public Guid ServerId { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        protected bool ShowDialog { get; set; } = false;
        protected bool ShowLoader { get; set; } = false;
        protected Models.Server Server { get; set; } = new Models.Server();
        protected string Token { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Token = await AccountService.GenerateToken();
                StateHasChanged();
            }
        }

        protected async void ServerSaved()
        {
            await SwalService.ShowInfoWithConfirmOk("Done", "Done");
        }
    }
}
