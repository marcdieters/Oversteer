using Microsoft.AspNetCore.Components;
using Oversteer.Models;

namespace Oversteer.BlazorComponents.Championship
{
    public partial class Generic
    {
        [Parameter]
        public Models.Racing.Championship Championship { get; set; } = new Models.Racing.Championship();
        [Parameter]
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        [Parameter]
        public string SelectedRaceSimId { get; set; } = string.Empty;
        [Parameter]
        public EventCallback<string> SelectedRaceSimIdChanged { get; set; }

        protected async void RaceSimSelected()
        {
            await SelectedRaceSimIdChanged.InvokeAsync(SelectedRaceSimId);
        }
    }
}
