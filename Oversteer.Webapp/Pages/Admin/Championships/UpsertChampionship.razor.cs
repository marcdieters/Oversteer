using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Models.Racing;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Championships
{
    public partial class UpsertChampionship
    {
        [Inject]
        protected IAccountService AccountService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "id")]
        public Guid Id { get; set; }

        protected string Token { get; set; } = string.Empty;
        protected bool Loaded { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Token = await AccountService.GenerateToken();
            Loaded = true;
            StateHasChanged();
        }

        protected async void ChampionshipSaved()
        {
            await SwalService.ShowInfoWithConfirmOk("Done", "Done");
        }
    }
}
