using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.DLC
{
    public partial class _UpsertDlc
    {
        [Inject]
        public IDLCService DLCService { get; set; }
        [Inject]
        public IRaceSimService RaceSimService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Models.Dlc Dlc { get; set; } = new Models.Dlc();
        public List<Models.RaceSim> RaceSims { get; set; } = new List<Models.RaceSim>();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public async Task ShowAsync(Models.Dlc dlc)
        {
            Dlc = dlc;
            RaceSims = await RaceSimService.GetAllRaceSims();
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
                await DLCService.UpsertDlc(Dlc);

                ShowLoader = false;
                StateHasChanged();

                var confirm = await Swal.ShowInfoWithConfirmOk("Dlc saved", "This dlc has been saved succesfully.");
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
