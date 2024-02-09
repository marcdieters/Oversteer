using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Models.Racing;
using Oversteer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Championship
{
    public partial class Splits
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }
        protected ICarService? CarService { get; set; }

        [Parameter]
        public List<Split> ChampionshipSplits { get; set; } = new List<Split>();

        protected List<CarClass> CarClasses { get; set; } = new List<CarClass>();
        protected Split SelectedSplit { get; set; } = new Split();

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                CarService = new CarService(NavigationManager!.BaseUri, Globals.Token);
                CarClasses = await CarService!.GetCarClasses();
                StateHasChanged();

                if (ChampionshipSplits.Count == 0)
                {
                    Split split = new Split();
                    ChampionshipSplits.Add(split);
                    StateHasChanged();
                }
            }
        }

        protected void AddSplit()
        {
            Split split = new Split();
            ChampionshipSplits.Add(split);
            StateHasChanged();
        }

        protected void RemoveSplit(Guid id)
        {
            if (ChampionshipSplits.Any(c => c.Id == id))
            {
                var splitToRemove = ChampionshipSplits.First(c => c.Id == id);
                ChampionshipSplits.Remove(splitToRemove);
                StateHasChanged();
            }
        }
    }
}
