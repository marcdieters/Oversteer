using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Races
{
    public partial class UploadResult
    {
        [Inject]
        protected IAccountService AccountService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "raceid")]
        public Guid RaceId { get; set; }

        protected string Token { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Token = await AccountService.GenerateToken();
                StateHasChanged();
            }
        }

        protected async void ResultsUploaded()
        {
            await SwalService.ShowInfoWithConfirmOk("Done", "Done");
        }
    }
}
