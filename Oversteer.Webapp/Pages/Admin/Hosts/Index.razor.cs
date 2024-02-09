using Microsoft.AspNetCore.Components;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Hosts
{
    public partial class Index
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected IAccountService AccountService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }
        protected IHostService HostService { get; set; }

        protected _UpsertHost _UpsertHost { get; set; }

        protected string Token { get; set; } = string.Empty;
        protected bool ShowLoader { get; set; }
        protected bool ShowDialog { get; set; }
        protected List<Models.Host> Hosts { get; set; } = new List<Models.Host>();
        protected Models.Host SelectedHost { get; set; } = new Models.Host();

        protected override async Task OnInitializedAsync()
        {
            Token = await AccountService.GenerateToken();
            HostService = new HostService(NavigationManager!.BaseUri, Token);
            Hosts = await HostService.GetHosts(Guid.Empty);
        }
        
        protected async void UpsertHost(bool addNew)
        {
            if (addNew)
            {
                string key = await HostService.GetHostKey();
                var confirm = await SwalService.ShowInfoWithConfirmOk("Setup host", @$"Start your new host and enter the following security key. 
                    {Environment.NewLine}{Environment.NewLine}
                    {key}{Environment.NewLine}{Environment.NewLine}
                    Valid until {DateTime.Now.AddMinutes(30)}. Press ok when done.");

                if (confirm.IsConfirmed)
                {
                    Hosts = await HostService.GetHosts(Guid.Empty);
                }
            }
            else
            {
                await _UpsertHost.ShowAsync(SelectedHost);
            }
        }

        protected void ShowOptions(Models.Host host)
        {
            SelectedHost = host;
            ShowDialog = true;
            StateHasChanged();
        }

        protected void RemoveHost()
        {

        }

        protected async void UpsertHostDialog_OnDialogClose()
        {
            Hosts = await HostService.GetHosts(Guid.Empty);
        }

        protected void Close()
        {
            ShowDialog = false;
        }
    }
}
