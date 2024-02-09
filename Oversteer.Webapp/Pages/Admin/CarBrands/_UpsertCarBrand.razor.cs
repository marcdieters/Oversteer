using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;
using Microsoft.AspNetCore.Authorization;

namespace Oversteer.Webapp.Pages.Admin.CarBrands
{
    [Authorize(Roles = "Admins")]
    public partial class _UpsertCarBrand
    {
        [Inject]
        public ICountryService CountryService { get; set; }
        [Inject]
        public ICarService CarService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Models.CarBrand CarBrand { get; set; } = new Models.CarBrand();
        public List<Models.Country> Countries { get; set; } = new List<Models.Country>();
        public IReadOnlyList<IBrowserFile> SelectedFiles { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public string Message { get; set; }

        public async Task ShowAsync(Models.CarBrand carBrand)
        {
            CarBrand = carBrand;
            Countries = await CountryService.GetAllCountries();
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
                if (SelectedFiles != null)
                {
                    ShowLoader = true;

                    string ext = Path.GetExtension(SelectedFiles[0].Name);
                    CarBrand.Logo = Guid.NewGuid().ToString() + ext;

                    Stream stream = SelectedFiles[0].OpenReadStream();
                    MemoryStream ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    stream.Close();

                    byte[] fileContent = ms.ToArray();
                    ms.Close();

                    await CarService.UpsertCarBrand(CarBrand, fileContent);

                    ShowLoader = false;
                    StateHasChanged();

                    var confirm = await Swal.ShowInfoWithConfirmOk("Carbrand saved", "This carbrand has been saved succesfully.");
                    if (confirm.IsConfirmed)
                    {
                        ShowDialog = false;
                        await CloseEventCallback.InvokeAsync(true);
                    }
                }
                else
                {
                    Message = "A logo is required for this";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            SelectedFiles = e.GetMultipleFiles();
            Message = $"{SelectedFiles.Count} file(s) selected";
        }
    }
}
