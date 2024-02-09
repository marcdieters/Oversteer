using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Country
{
    [Authorize(Roles = "Admins")]
    public partial class _UpsertCountry
    {
        [Inject]
        public ICountryService CountryService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Models.Country Country { get; set; } = new Models.Country();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public async Task ShowAsync(Models.Country country)
        {
            Country = country;
            ShowDialog = true;
        }

        public async Task Close()
        {
            ShowDialog = false;
            ShowLoader = false;

            StateHasChanged();
            await CloseEventCallback.InvokeAsync(true);
        }

        protected async void HandleValidSubmit()
        {
            try
            {
                ShowLoader = true;
                await CountryService.UpsertCountry(Country);

                ShowLoader = false;
                StateHasChanged();

                var confirm = await Swal.ShowInfoWithConfirmOk("Country saved", "This country has been saved succesfully.");
                if (confirm.IsConfirmed)
                {
                    ShowDialog = false;
                    await CloseEventCallback.InvokeAsync(true);
                }
            }
            catch (Exception ex)
            {
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }
    }
}
