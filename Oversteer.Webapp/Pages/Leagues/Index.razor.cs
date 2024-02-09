using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Leagues
{
    public partial class Index
    {
        [Inject]
        public ILeagueService LeagueService { get; set; }
        [Inject]
        public ISwalService? Swal { get; set; }

        public _LeagueRegistration _LeagueRegistration { get; set; }

        public List<League> UnfilteredLeagues { get; set; } = new List<League>();
        public List<League> Leagues { get; set; } = new List<League>();
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                UnfilteredLeagues = await LeagueService.GetLeagues(LeagueOrder.Name);
                Leagues = new List<League>(UnfilteredLeagues);
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                Leagues = new List<League>();
                UnfilteredLeagues = new List<League>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void UpsertLeagueRegistration(bool newRecord)
        {
            _LeagueRegistration.Show();
        }
    }
}

