using Microsoft.AspNetCore.Components;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Server
{
    public partial class Index
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected IAccountService AccountService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }
        protected IServerService ServerService { get; set; }

        protected _UpsertServer _UpsertServer { get; set; }

        protected string Token { get; set; } = string.Empty;
        protected bool ShowLoader { get; set; }
        protected bool ShowDialog { get; set; }
        protected List<Models.Server> Servers { get; set; } = new List<Models.Server>();
        protected Models.Server SelectedServer { get; set; } = new Models.Server();

        protected override async Task OnInitializedAsync()
        {
            Token = await AccountService.GenerateToken();
            ServerService = new ServerService(NavigationManager!.BaseUri, Token);
            Servers = await ServerService.GetServers();
        }

        protected async void UpsertServer(bool addNew)
        {
            if (addNew)
            {
                NavigationManager.NavigateTo($"upsertserver");
            }
            else
            {
                NavigationManager.NavigateTo($"upsertserver?serverid={SelectedServer.Id}");
            }
        }

        protected void ShowOptions(Models.Server server)
        {
            SelectedServer = server;
            ShowDialog = true;
        }

        protected void RemoveServer()
        {

        }

        protected async void UpsertServerDialog_OnDialogClose()
        {
            Servers = await ServerService.GetServers();
        }

        protected void Close()
        {
            ShowDialog = false;
        }
    }
}
