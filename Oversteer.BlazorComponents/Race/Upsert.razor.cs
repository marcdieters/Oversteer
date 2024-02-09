using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Models.Racing;
using Oversteer.Services;
using Oversteer.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Race
{
    public partial class Upsert
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }
        protected IChampionshipService? ChampionshipService { get; set; }
        protected IRaceService? RaceService { get; set; }
        protected IRaceSimService? RaceSimService { get; set; }

        [Parameter]
        public Guid ChampionshipId { get; set; }
        [Parameter]
        public Guid RaceId { get; set; }
        [Parameter]
        public EventCallback<Guid> RaceIdChanged { get; set; }
        [Parameter]
        public string Token { get; set; } = string.Empty;
        [Parameter]
        public bool ShowWeatherSettings { get; set; }

        protected List<Models.Racing.Championship> Championships { get; set; } = new List<Models.Racing.Championship>();
        protected Models.Racing.Race Race { get; set; } = new Models.Racing.Race();
        protected List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        protected string SelectedRaceSimId { get; set; } = string.Empty;
        protected bool HideAccTab { get; set; } = true;
        protected bool IsChampionShipRace { get; set; }

        protected override async void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                Globals.Token = Token;
                RaceService = new RaceService(NavigationManager!.BaseUri, Token);
                RaceSimService = new RaceSimService(NavigationManager!.BaseUri, Token);
                RaceSims = await RaceSimService.GetAllRaceSims();

                ChampionshipService = new ChampionshipService(NavigationManager!.BaseUri, Globals.Token);
                Championships = await ChampionshipService!.GetChampionships();

                if (ChampionshipId != Guid.Empty)
                {
                    Race.ChampionshipId = ChampionshipId;
                    Race.Championship = await ChampionshipService!.GetChampionship(ChampionshipId);
                    Race.RaceSimId = Race.Championship.RaceSimId;
                    Race.RaceSim = RaceSims.First(r => r.Id == Race.RaceSimId);
                    SelectedRaceSimId = Race.RaceSimId.ToString();
                    IsChampionShipRace = true;
                    RaceSimSelected();
                }
                else
                {
                    IsChampionShipRace = false;
                }

                if (RaceId != Guid.Empty)
                {
                    Race = await RaceService.GetRace(RaceId);
                    SelectedRaceSimId = Race.RaceSimId.ToString();
                }

                StateHasChanged();
            }
        }

        protected async void HandleValidSubmit()
        {
            Race.Championship = null;
            Race.RaceSim = null;

            await RaceService!.PostRace(Race);
            await RaceIdChanged.InvokeAsync(Race.Id);
        }

        protected void RaceSimSelected()
        {
            Race.RaceSim = RaceSims.First(r => r.Id == Guid.Parse(SelectedRaceSimId));
            switch (Race.RaceSim.Name)
            {
                case "Assetto Corsa Compitizione":
                    HideAccTab = false;
                    break;
            }

            Race.RaceSimId = Guid.Parse(SelectedRaceSimId);
        }
    }
}
