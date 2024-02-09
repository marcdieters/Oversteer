using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Pages.Admin.Country;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.CarClasses
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {
        [Inject]
        public ICarService CarService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        protected _UpsertCarClass? _UpsertCarClass { get; set; }

        public List<Models.CarClass> CarClasses { get; set; } = new List<Models.CarClass>();
        public Models.CarClass SelectedCarClass { get; set; } = new Models.CarClass();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                CarClasses = await CarService.GetCarClasses();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                CarClasses = new List<Models.CarClass>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void ShowOptions(Models.CarClass carclass)
        {
            SelectedCarClass = carclass;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertCarClass(bool newrecord)
        {
            if (newrecord)
            {
                SelectedCarClass = new Models.CarClass();
            }

            await _UpsertCarClass.ShowAsync(SelectedCarClass);
        }

        protected async Task UpsertCarClassDialog_OnDialogClose()
        {
            try
            {
                CarClasses = await CarService.GetCarClasses();
            }
            catch (Exception ex)
            {
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected async Task RemoveCarClass()
        {
            try
            {
                var confirm = await Swal.ShowInfoWithConfirm("Remove carclass?", "Are you sure you want to remove this carclass?");

                if (confirm.IsConfirmed)
                {
                    await CarService.RemoveCarClass(SelectedCarClass);
                    CarClasses.Remove(SelectedCarClass);
                    ShowDialog = false;
                }
            }
            catch (Exception ex)
            {
                CarClasses = new List<Models.CarClass>();
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
