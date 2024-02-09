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
    public partial class SplitEntry
    {
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        public ICarService? CarService { get; set; }

        [Parameter]
        public Split Split { get; set; } = new Split();
        [Parameter]
        public List<CarClass> CarClasses { get; set; } = new List<CarClass>();
        protected List<Car> Cars { get; set; } = new List<Car>();
        protected bool ShowSelectCars { get; set; }

        protected override void OnParametersSet()
        {
            CarService = new CarService(NavigationManager!.BaseUri, Globals.Token);
        }

        protected async void GetCars()
        {
            if (Split.CarClassId != Guid.Empty)
            {
                Cars = await CarService!.GetCarsInClass(Split.CarClassId);
                ShowSelectCars = true;
            }
            else
            {
                ShowSelectCars = false;
            }

            StateHasChanged();
        }

        protected void CarSelected()
        {
            if (Cars.Any(c => c.Selected))
            {
                Split.ManuallySelectedCars = true;
            }
            else
            {
                Split.ManuallySelectedCars = false;
            }
        }
    }
}
