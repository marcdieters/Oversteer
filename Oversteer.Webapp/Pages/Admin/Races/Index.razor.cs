using Microsoft.AspNetCore.Components;
using Oversteer.Models.Racing;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Races
{
    public partial class Index
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected IAccountService AccountService { get; set; }
        [Inject]
        protected ISwalService SwalService { get; set; }
        protected IRaceService RaceService { get; set; }

        protected List<Race> Races { get; set; } = new List<Race>();
        protected Race SelectedRace { get; set; } = new Race();
        protected bool ShowLoader { get; set; } = true;
        protected bool ShowDialog { get; set; }
        protected bool ShowUploadResultDialog { get; set; }
        protected string Token { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Token = await AccountService.GenerateToken();
            RaceService = new RaceService(NavigationManager!.BaseUri, Token);
            Races = await RaceService.GetRaces();
            ShowLoader = false;
            StateHasChanged();
        }

        protected void ShowOptions(Race race)
        {
            SelectedRace = race;
            ShowDialog = true;
            StateHasChanged();
        }

        protected void Upsert(bool addNew)
        {
            if (addNew)
            {
                NavigationManager.NavigateTo($"upsertrace");
            }
            else
            {
                NavigationManager.NavigateTo($"upsertrace?raceid={SelectedRace.Id}");
            }
        }

        protected void UploadResult()
        {
            NavigationManager.NavigateTo($"uploadresult?raceid={SelectedRace.Id}");
        }

        protected void OpenRow(Race race)
        {
            NavigationManager.NavigateTo($"upsertrace?RaceId={race.Id}");
        }

        protected void Remove()
        {

        }
    }
}
