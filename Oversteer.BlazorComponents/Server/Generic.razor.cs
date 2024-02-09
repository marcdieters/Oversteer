using Microsoft.AspNetCore.Components;
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

namespace Oversteer.BlazorComponents.Server
{
    public partial class Generic
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        protected IServerService? ServerService { get; set; }
        protected ITrackService? TrackService { get; set; }

        [Parameter]
        public string Token { get; set; } = string.Empty;
        [Parameter]
        public Models.Server Server { get; set; } = new Models.Server();
        [Parameter]
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        [Parameter]
        public List<Host> Hosts { get; set; } = new List<Host>();
        [Parameter]
        public List<CarClass> CarClasses { get; set; } = new List<CarClass>();
        [Parameter]
        public string SelectedRaceSimId { get; set; } = string.Empty;
        [Parameter]
        public EventCallback<string> SelectedRaceSimIdChanged { get; set; }

        protected List<Track> Tracks { get; set; } = new List<Track>();
        protected List<TrackLayout> TrackLayouts { get; set; } = new List<TrackLayout>();

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                Globals.Token = Token;
                TrackService = new TrackService(NavigationManager!.BaseUri, Globals.Token);

                if (Server.RaceSimId != Guid.Empty)
                {
                    SelectedRaceSimId = Server.RaceSimId.ToString();
                }

                StateHasChanged();
            }
        }

        protected void TrackSelected()
        {
            if (Server.RaceSimId != Guid.Empty)
            {
                var raceSim = RaceSims.First(r => r.Id == Server.RaceSimId);
                if (raceSim.DoesSimSupportTrackLayouts)
                {
                    TrackLayouts = Tracks.First(t => t.Id == Server.TrackId).TrackLayouts;
                }
            }
        }

        protected async void RaceSimSelected()
        {
            await SelectedRaceSimIdChanged.InvokeAsync(SelectedRaceSimId);
            Server.RaceSimId = Guid.Parse(SelectedRaceSimId);
            Tracks = await TrackService!.GetTracksInRaceSim(Server.RaceSimId);
            StateHasChanged();
        }
    }
}
