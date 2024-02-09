using Microsoft.AspNetCore.Components;
using Oversteer.Models.Racing;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Races
{
    public partial class UpsertRace
    {
        [Inject]
        protected IAccountService AccountService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "raceid")]
        public Guid RaceId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "championshipid")]
        public Guid ChampionshipId { get; set; }

        protected string Token { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Token = await AccountService.GenerateToken();
                StateHasChanged();
            }
        }

        protected async void RaceSaved()
        {
            await SwalService.ShowInfoWithConfirmOk("Done", "Done");
        }
    }
}
