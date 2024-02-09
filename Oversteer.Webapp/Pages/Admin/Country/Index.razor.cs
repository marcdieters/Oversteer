using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;
using Oversteer.Models;
using Org.BouncyCastle.Asn1.Cmp;
using Microsoft.AspNetCore.Authorization;

namespace Oversteer.Webapp.Pages.Admin.Country
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {
        [Inject]
        public ICountryService CountryService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected _UpsertCountry? _UpsertCountry { get; set; }

        public List<Models.Country> Countries { get; set; } = new List<Models.Country>();
        public Models.Country SelectedCountry { get; set; } = new Models.Country();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                Countries = await CountryService.GetAllCountries();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                Countries = new List<Models.Country>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void ShowOptions(Models.Country country)
        {
            SelectedCountry = country;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertCountry(bool newrecord)
        {
            if (newrecord)
            {
                SelectedCountry = new Models.Country();
            }

            await _UpsertCountry.ShowAsync(SelectedCountry);
        }

        protected async Task UpsertCountryDialog_OnDialogClose()
        {
            try
            {
                Countries = await CountryService.GetAllCountries();
            }
            catch (Exception ex)
            {
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected async Task RemoveCountry()
        {
            try
            {
                var confirm = await Swal.ShowInfoWithConfirm("Remove country?", "Are you sure you want to remove this country?");

                if (confirm.IsConfirmed)
                {
                    await CountryService.RemoveCountry(SelectedCountry);
                    Countries.Remove(SelectedCountry);
                    ShowDialog = false;
                }
            }
            catch (Exception ex)
            {
                Countries = new List<Models.Country>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void Close()
        {
            ShowDialog = false;
        }

        protected void Test()
        {
            NavigationManager.NavigateTo("/carclasses", true);
        }
    }
}
