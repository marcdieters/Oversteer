using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Pages.Admin.CarClasses;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.CarBrands
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {
        [Inject]
        public ICarService CarService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        protected _UpsertCarBrand? _UpsertCarBrand { get; set; }

        public List<Models.CarBrand> CarBrands { get; set; } = new List<Models.CarBrand>();
        public Models.CarBrand SelectedCarBrand { get; set; } = new Models.CarBrand();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                CarBrands = await CarService.GetCarBrands();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                CarBrands = new List<Models.CarBrand>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void ShowOptions(Models.CarBrand carclass)
        {
            SelectedCarBrand = carclass;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertCarBrand(bool newrecord)
        {
            if (newrecord)
            {
                SelectedCarBrand = new Models.CarBrand();
            }

            ShowLoader = true;
            StateHasChanged();
            await _UpsertCarBrand.ShowAsync(SelectedCarBrand);
            ShowLoader = false;
            StateHasChanged();
        }

        protected async Task UpsertCarBrandDialog_OnDialogClose()
        {
            try
            {
                CarBrands = await CarService.GetCarBrands();
            }
            catch (Exception ex)
            {
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected async Task RemoveCarBrand()
        {
            try
            {
                var confirm = await Swal.ShowInfoWithConfirm("Remove carbrand?", "Are you sure you want to remove this carbrand?");

                if (confirm.IsConfirmed)
                {
                    await CarService.RemoveCarBrand(SelectedCarBrand);
                    CarBrands.Remove(SelectedCarBrand);
                    ShowDialog = false;
                }
            }
            catch (Exception ex)
            {
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void Close()
        {
            ShowDialog = false;
        }
    }
}
