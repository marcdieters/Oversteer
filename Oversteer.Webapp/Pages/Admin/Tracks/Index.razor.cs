using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Pages.Admin.Cars;
using Oversteer.Webapp.Services;
using System.Data;

namespace Oversteer.Webapp.Pages.Admin.Tracks
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {
        [Inject]
        public ITrackService TrackService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        protected _UpsertTrack? _UpsertTrack { get; set; }

        public List<Models.Track> Tracks { get; set; } = new List<Models.Track>();
        public Models.Track SelectedTrack { get; set; } = new Models.Track();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                StateHasChanged();
                Tracks = await TrackService!.GetTracks();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                Tracks = new List<Models.Track>();
                ShowLoader = false;
                await Swal!.ShowError($"That didn't work. Error: {ex.Message}");
            }
            StateHasChanged();
        }

        protected void ShowOptions(Models.Track track)
        {
            SelectedTrack = track;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertTrack(bool newrecord)
        {
            if (newrecord)
            {
                SelectedTrack = new Models.Track();
            }

            await _UpsertTrack!.ShowAsync(SelectedTrack);
        }

        protected async Task UpsertTrackDialog_OnDialogClose()
        {
            try
            {
                Tracks = await TrackService!.GetTracks();
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
                var confirm = await Swal!.ShowInfoWithConfirm("Remove track?", "Are you sure you want to remove this track?");

                if (confirm.IsConfirmed)
                {
                    await TrackService!.RemoveTrack(SelectedTrack);
                    Tracks.Remove(SelectedTrack);
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
