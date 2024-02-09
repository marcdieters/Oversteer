using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Leagues
{
    [Authorize]
    public partial class _LeagueRegistration
    {
        [Inject]
        public ILeagueService? LeagueService { get; set; }
        [Inject]
        public ISwalService? Swal { get; set; }

        public League League { get; set; } = new League();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        public async void Show()
        {
            var existingRegistration = await LeagueService!.GetLeagueOfLoggedInUser();
            if (existingRegistration != null && existingRegistration.Approved == false)
            {
                var comfirm = await Swal!.ShowInfoWithConfirm("Existing registration", "You have already submitted a registration. Do you want to edit your existing registration ?");
                if (comfirm.IsConfirmed)
                {
                    League = existingRegistration;
                    ShowDialog = true;
                    StateHasChanged();
                }
                else
                {
                    return;
                }
            }
            else if (existingRegistration != null && existingRegistration.Approved == true)
            {
                await Swal!.ShowInfoWithConfirmOk("Already approved", "Your registration has already been approved");
                return;
            }

            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                ShowLoader = true;
                await LeagueService!.UpsertLeague(League, null, null);

                ShowLoader = false;
                StateHasChanged();

                var confirm = await Swal!.ShowInfoWithConfirmOk("Registration submitted", "Your registration has been submitted. Please join or discord to complete the registration");
                if (confirm.IsConfirmed)
                {
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
            StateHasChanged();
        }
    }
}
