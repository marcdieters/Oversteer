using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Hosts
{
    public partial class _UpsertHost
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        public ICountryService CountryService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }
        [Inject]
        protected IAccountService AccountService { get; set; }
        protected IHostService HostService { get; set; }

        protected bool ShowDialog { get; set; } = false;
        protected bool ShowLoader { get; set; } = false;
        protected Models.Host Host { get; set; } = new Models.Host();
        public List<Models.Country> Countries { get; set; } = new List<Models.Country>();
        protected string Token { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public string Message { get; set; }

        public async Task ShowAsync(Models.Host host)
        {
            Token = await AccountService.GenerateToken();
            HostService = new HostService(NavigationManager!.BaseUri, Token);

            Host = host;
            Countries = await CountryService.GetAllCountries();
            ShowDialog = true;
            StateHasChanged();
        }

        protected async void HandleValidSubmit()
        {
            await HostService.SaveHost(Host);

            ShowLoader = false;
            StateHasChanged();

            var confirm = await SwalService.ShowInfoWithConfirmOk("Host saved", "This host has been saved succesfully.");
            if (confirm.IsConfirmed)
            {
                ShowDialog = false;
                await CloseEventCallback.InvokeAsync(true);
            }
        }

        protected void Close()
        {
            ShowDialog = false;
        }
    }
}
