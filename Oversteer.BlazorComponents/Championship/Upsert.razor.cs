using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Oversteer.Models;
using Oversteer.Services;
using Oversteer.Services.Implementation;

namespace Oversteer.BlazorComponents.Championship
{
    public partial class Upsert
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }
        protected IChampionshipService? ChampionshipService { get; set; }
        protected IRaceSimService? RaceSimService { get; set; }

        [Parameter]
        public Guid ChampionshipId { get; set; }
        [Parameter]
        public EventCallback<Guid> ChampionshipIdChanged { get; set; }
        [Parameter]
        public string Token { get; set; } = string.Empty;

        protected Models.Racing.Championship Championship { get; set; } = new Models.Racing.Championship();
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        protected string SelectedRaceSimId { get; set; } = string.Empty;
        protected bool HideAccTab { get; set; } = true;

        protected override async void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                Globals.Token = Token;
                ChampionshipService = new ChampionshipService(NavigationManager!.BaseUri, Token);
                RaceSimService = new RaceSimService(NavigationManager!.BaseUri, Token);
                RaceSims = await RaceSimService.GetAllRaceSims();

                if (ChampionshipId != Guid.Empty)
                {
                    Championship = await ChampionshipService.GetChampionship(ChampionshipId);
                    SelectedRaceSimId = Championship.RaceSimId.ToString();
                    RaceSimSelected();
                }

                StateHasChanged();
            }
        }

        protected void RaceSimSelected()
        {
            RaceSim raceSim = RaceSims.First(r => r.Id == Guid.Parse(SelectedRaceSimId));
            switch (raceSim.Name)
            {
                case "Assetto Corsa Compitizione":
                    HideAccTab = false;
                    break;
            }

            Championship.RaceSimId = Guid.Parse(SelectedRaceSimId);
        }

        protected async void HandleValidSubmit()
        {
            Championship.Races.Clear();
            await ChampionshipService!.PostChampionship(Championship);
            await ChampionshipIdChanged.InvokeAsync(ChampionshipId);
        }
    }
}
