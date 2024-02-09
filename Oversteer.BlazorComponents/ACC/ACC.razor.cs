using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.ACC
{
    public partial class ACC
    {
        [Parameter]
        public Models.ACC Acc { get; set; } = new Models.ACC();
        [Parameter]
        public bool IsChampionshipRace { get; set; }
        [Parameter]
        public bool ShowWeatherSettings { get; set; }

        public bool Loaded { get; set; }

        protected override void OnParametersSet()
        {
            Loaded = true;
        }
    }
}
