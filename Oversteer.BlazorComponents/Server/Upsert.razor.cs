using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Services;
using Oversteer.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Server
{
    public partial class Upsert
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }
        protected IServerService? ServerService { get; set; }
        protected IRaceSimService? RaceSimService { get; set; }
        protected IHostService? HostService { get; set; }
        protected ICarService? CarService { get; set; }

        [Parameter]
        public Guid LeagueId { get; set; }
        [Parameter]
        public string Token { get; set; } = string.Empty;
        [Parameter]
        public Guid ServerId { get; set; }
        [Parameter]
        public EventCallback<Guid> ServerIdChanged { get; set; }

        public List<CarClass> CarClasses { get; set; } = new List<CarClass>();
        public List<Host> Hosts { get; set; } = new List<Host>();
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        protected Models.Server Server { get; set; } = new Models.Server();
        protected string SelectedRaceSimId { get; set; } = string.Empty;
        protected bool HideAccTab { get; set; } = true;

        protected override async void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                Globals.Token = Token;
                ServerService = new ServerService(NavigationManager!.BaseUri, Token);
                RaceSimService = new RaceSimService(NavigationManager!.BaseUri, Token);
                HostService = new HostService(NavigationManager!?.BaseUri, Token);
                CarService = new CarService(NavigationManager!?.BaseUri, Token);

                RaceSims = await RaceSimService.GetAllRaceSims();
                Hosts = await HostService.GetHosts(LeagueId);
                CarClasses = await CarService.GetCarClasses();

                if (ServerId != Guid.Empty)
                {
                    Server = await ServerService.GetServer(ServerId);
                }

                StateHasChanged();
            }
        }

        protected  void RaceSimSelected()
        {
            var raceSim = RaceSims.First(r => r.Id == Guid.Parse(SelectedRaceSimId));
            switch (raceSim.Name)
            {
                case "Assetto Corsa Compitizione":
                    HideAccTab = false;
                    break;
            }

            Server.RaceSimId = Guid.Parse(SelectedRaceSimId);
        }

        protected async void HandleValidSubmit()
        {
            await ServerService!.PostServer(Server);
            await ServerIdChanged.InvokeAsync(Server.Id);
        }
    }
}
