using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Pages.Admin.Country;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Cars
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {
        [Inject]
        public ICarService? CarService { get; set; }
        [Inject]
        public ISwalService? Swal { get; set; }

        protected _UpsertCar? _UpsertCar { get; set; }

        public List<Models.Car> Cars { get; set; } = new List<Models.Car>();
        public Models.Car SelectedCar { get; set; } = new Models.Car();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                StateHasChanged();
                Cars = await CarService!.GetCars();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                Cars = new List<Models.Car>();
                ShowLoader = false;
                await Swal!.ShowError($"That didn't work. Error: {ex.Message}");
            }
            StateHasChanged();
        }

        protected void ShowOptions(Models.Car country)
        {
            SelectedCar = country;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertCountry(bool newrecord)
        {
            if (newrecord)
            {
                SelectedCar = new Models.Car();
            }

            await _UpsertCar!.ShowAsync(SelectedCar);
        }

        protected async Task UpsertCarDialog_OnDialogClose()
        {
            try
            {
                Cars = await CarService!.GetCars();
            }
            catch (Exception ex)
            {
                StateHasChanged();
                await Swal!.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected async Task RemoveCountry()
        {
            try
            {
                var confirm = await Swal!.ShowInfoWithConfirm("Remove country?", "Are you sure you want to remove this country?");

                if (confirm.IsConfirmed)
                {
                    await CarService!.RemoveCar(SelectedCar);
                    Cars.Remove(SelectedCar);
                    ShowDialog = false;
                }
            }
            catch (Exception ex)
            {
                ShowLoader = false;
                StateHasChanged();
                await Swal!.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void Close()
        {
            ShowDialog = false;
        }
    }
}
