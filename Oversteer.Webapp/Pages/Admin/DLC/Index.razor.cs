using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;
using Oversteer.Models;
using Org.BouncyCastle.Asn1.Cmp;
using Microsoft.AspNetCore.Authorization;

namespace Oversteer.Webapp.Pages.Admin.DLC
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {
        [Inject]
        public IDLCService DLCService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        protected _UpsertDlc? _UpsertDlc { get; set; }

        public List<Models.Dlc> Dlcs { get; set; } = new List<Models.Dlc>();
        public Models.Dlc SelectedDlc { get; set; } = new Models.Dlc();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                Dlcs = await DLCService.GetDLCs();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                Dlcs = new List<Models.Dlc>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void ShowOptions(Models.Dlc dlc)
        {
            SelectedDlc = dlc;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertDlc(bool newrecord)
        {
            if (newrecord)
            {
                SelectedDlc = new Models.Dlc();
            }

            await _UpsertDlc.ShowAsync(SelectedDlc);
        }

        protected async Task UpsertDlcDialog_OnDialogClose()
        {
            try
            {
                Dlcs = await DLCService.GetDLCs();
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
                    await DLCService.RemoveDlc(SelectedDlc);
                    Dlcs.Remove(SelectedDlc);
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
