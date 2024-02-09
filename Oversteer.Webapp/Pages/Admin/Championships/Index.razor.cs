using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Oversteer.Models.Racing;
using Oversteer.Services;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Championships
{
    public partial class Index
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected IAccountService AccountService { get; set; }
        protected IChampionshipService ChampionshipService { get; set; }

        protected List<Championship> Championships { get; set; } = new List<Championship>();
        protected Championship SelectedChampionship { get; set; } = new Championship();
        public bool ShowLoader { get; set; } = true;
        protected bool ShowDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            string token = await AccountService.GenerateToken();
            ChampionshipService = new ChampionshipService(NavigationManager!.BaseUri, token);
            Championships = await ChampionshipService.GetChampionships();
            ShowLoader = false;
            StateHasChanged();
        }

        protected void Upsert(bool addNew)
        {
            if (addNew)
            {
                NavigationManager.NavigateTo($"upsertchampionship");
            }
            else
            {
                NavigationManager.NavigateTo($"upsertchampionship?id={SelectedChampionship.Id}");
            }
        }

        protected void Remove()
        {

        }

        protected void UpsertChampionship_OnDialogClose()
        {

        }

        protected void ShowOptions(Championship championship)
        {
            SelectedChampionship = championship;
            ShowDialog = true;
            StateHasChanged();
        }

        protected void OpenRow(Championship championship)
        {
            NavigationManager.NavigateTo($"upsertchampionship?id={championship.Id}");
        }

        protected void AddRace()
        {
            NavigationManager.NavigateTo($"upsertrace?championshipid={SelectedChampionship.Id}");
        }
    }
}
