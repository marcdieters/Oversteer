using Microsoft.AspNetCore.Components;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Championship
{
    public partial class Races
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        [Parameter]
        public Models.Racing.Championship Championship { get; set; } = new Models.Racing.Championship();

        protected bool ShowNewRaceDialog { get; set; }
        protected Models.Racing.Race Race { get; set; } = new Models.Racing.Race();

        protected void AddRace()
        {
            Race.RaceSimId = Championship.RaceSimId;
            Race.ChampionshipId = Championship.Id;
            ShowNewRaceDialog = true;
            NavigationManager!.NavigateTo($"upsertrace?championshipid={Championship.Id}");
        }
    }
}
