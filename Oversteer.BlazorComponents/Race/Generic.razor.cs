using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Oversteer.Helpers;
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
    public partial class Generic
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }
        protected ITrackService? TrackService { get; set; }

        [Parameter]
        public List<Models.Racing.Championship> Championships { get; set; } = new List<Models.Racing.Championship>();
        [Parameter]
        public Models.Racing.Race Race { get; set; } = new Models.Racing.Race();
        [Parameter]
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        [Parameter]
        public string SelectedRaceSimId { get; set; } = string.Empty;
        [Parameter]
        public EventCallback<string> SelectedRaceSimIdChanged { get; set; }
        [Parameter]
        public bool IsChampionShipRace { get; set; }

        protected List<Track> Tracks { get; set; } = new List<Track>();
        protected List<TrackLayout> TrackLayouts { get; set; } = new List<TrackLayout>();
        protected string FileMessage { get; set; } = string.Empty;
        public IReadOnlyList<IBrowserFile>? SelectedFiles { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            TrackService = new TrackService(NavigationManager!.BaseUri, Globals.Token);

            if (Race.ChampionshipId != Guid.Empty)
            {
                IsChampionShipRace = true;
            }
            else
            {
                IsChampionShipRace = false;
            }

            if (Race.RaceSimId != Guid.Empty)
            {
                Tracks = await TrackService!.GetTracksInRaceSim(Race.RaceSimId);
            }
        }

        protected async void RaceSimSelected()
        {
            Race.RaceSim = RaceSims.First(r => r.Id == Guid.Parse(SelectedRaceSimId));
            await SelectedRaceSimIdChanged.InvokeAsync(SelectedRaceSimId);
            Tracks = await TrackService!.GetTracksInRaceSim(Race.RaceSimId);
            StateHasChanged();
        }

        protected void TrackSelected()
        {
            if (Race.RaceSim.DoesSimSupportTrackLayouts)
            {
                TrackLayouts = Tracks.First(t => t.Id == Race.TrackId).TrackLayouts;
            }
        }

        protected async void ChampionshipSelected()
        {
            if (Race.ChampionshipId != Guid.Empty)
            {
                var selChampionship = Championships.First(a => a.Id == Race.ChampionshipId);
                Race.RaceSimId = RaceSims.First(r => r.Id == selChampionship.RaceSimId).Id;
                SelectedRaceSimId = Race.RaceSimId.ToString();
                Tracks = await TrackService!.GetTracksInRaceSim(Race.RaceSimId);
                StateHasChanged();
            }
        }

        protected void NameEntered()
        {
            
        }
    }
}
