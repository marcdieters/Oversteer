using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Build.Framework;
using NuGet.Protocol;
using Oversteer.Models;
using Oversteer.Webapp.Services;
using System;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Oversteer.Webapp.Pages.Admin.CarClasses
{
    [Authorize(Roles = "Admins")]
    public partial class _UpsertCarClass
    {
        [Inject]
        public ICarService CarService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Models.CarClass CarClass { get; set; } = new Models.CarClass();
        public IReadOnlyList<IBrowserFile> SelectedFiles { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public string Message { get; set; }

        public async Task ShowAsync(Models.CarClass carClass)
        {
            CarClass = carClass;
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
                    CarClass.Logo = Guid.NewGuid().ToString() + ext;

                    Stream stream = SelectedFiles[0].OpenReadStream();
                    MemoryStream ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    stream.Close();

                    byte[] fileContent = ms.ToArray();
                    ms.Close();

                    await CarService.UpsertCarClass(CarClass, fileContent);

                    ShowLoader = false;
                    StateHasChanged();

                    var confirm = await Swal.ShowInfoWithConfirmOk("Carclass saved", "This carclass has been saved succesfully.");
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
