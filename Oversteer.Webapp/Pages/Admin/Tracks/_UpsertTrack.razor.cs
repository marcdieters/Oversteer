using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Tracks
{
    public partial class _UpsertTrack
    {
        [Inject]
        public ITrackService TrackService { get; set; }
        [Inject]
        public IDLCService DLCService { get; set; }
        [Inject]
        public IRaceSimService RaceSimService { get; set; }
        [Inject]
        public ICountryService CountryService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }
        [Inject]
        public IImageService ImageService { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;

        public List<TrackInRaceSim> TrackInRaceSims { get; set; } = new List<TrackInRaceSim>();
        public List<TrackLayout> TrackLayouts { get; set; } = new List<TrackLayout>();
        public List<Models.Country> Countries { get; set; } = new List<Models.Country>();
        public List<Dlc> Dlcs { get; set; } = new List<Dlc>();
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        public Track Track { get; set; } = new Track();
        public IReadOnlyList<IBrowserFile> SelectedFiles { get; set; }
        public IReadOnlyList<IBrowserFile> SelectedLayoutFiles { get; set; }
        public Guid SelectedRaceSimId { get; set; }
        public Guid SelectedTrackLayoutId { get; set; }
        public bool DisableDlcSelect { get; set; } = true;

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public string FileMessage { get; set; }
        public string TrackInSimMessage { get; set; }

        public async Task ShowAsync(Track track)
        {
            Track = track;
            RaceSims = await RaceSimService.GetAllRaceSims();
            Dlcs = await DLCService.GetDLCs();
            Countries = await CountryService.GetAllCountries();

            if (Track.Id != Guid.Empty)
            {
                TrackInRaceSims = RaceSimService.GetAllTracksInRaceSim(Track.Id);
                TrackLayouts = new List<TrackLayout>(Track.TrackLayouts);
            }

            ShowDialog = true;
        }

        public async Task Close()
        {
            ShowDialog = false;
            ShowLoader = false;

            StateHasChanged();
            await CloseEventCallback.InvokeAsync(true);
        }

        protected async Task SaveTrack(EditContext formContext)
        {
            try
            {
                bool formIsValid = formContext.Validate();
                if (formIsValid == false)
                    return;

                if (SelectedFiles != null && TrackInRaceSims.Count > 0 && TrackLayouts.Count > 0)
                {
                    ShowLoader = true;

                    string ext = Path.GetExtension(SelectedFiles[0].Name);
                    Track.SceneryImage = Guid.NewGuid().ToString() + ext;

                    Stream stream = SelectedFiles[0].OpenReadStream(1024000);
                    MemoryStream ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    stream.Close();

                    byte[] fileContent = ms.ToArray();
                    ms.Close();

                    Guid trackid = await TrackService.UpsertTrack(Track, fileContent);
                    foreach (var trackInSim in TrackInRaceSims)
                    {
                        trackInSim.TrackId = trackid;
                        await RaceSimService.SaveTrackInSim(trackInSim);
                    }

                    foreach (var layout in TrackLayouts)
                    {
                        layout.TrackId = trackid;
                        await TrackService.UpsertTrackLayout(layout);
                    }

                    ShowLoader = false;
                    StateHasChanged();

                    var confirm = await Swal.ShowInfoWithConfirmOk("Track saved", "This track has been saved succesfully.");
                    if (confirm.IsConfirmed)
                    {
                        ShowDialog = false;
                        await CloseEventCallback.InvokeAsync(true);
                    }
                }
                else
                {
                    if (SelectedFiles == null)
                    {
                        FileMessage = "An image is required for this";
                    }
                    else
                    {
                        FileMessage = "";
                    }

                    if (TrackInRaceSims.Count == 0)
                    {
                        TrackInSimMessage = "You need to specify at least one race sim.";
                    }
                    else
                    {
                        TrackInSimMessage = "";
                    }

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
            FileMessage = $"{SelectedFiles.Count} file(s) selected";
        }

        private void OnInputLayoutChange(InputFileChangeEventArgs e, string row)
        {
            var selectedLayoutFiles = e.GetMultipleFiles();
            int rowId = Convert.ToInt32(row);
            var layoutRecord = TrackLayouts.First(t => t.FieldSelector == rowId);

            string ext = Path.GetExtension(selectedLayoutFiles[0].Name);
            layoutRecord.LayoutImage = Guid.NewGuid().ToString() + ext;

            Stream stream = selectedLayoutFiles[0].OpenReadStream();
            MemoryStream ms = new MemoryStream();
            stream.CopyToAsync(ms).GetAwaiter();
            stream.Close();

            byte[] fileContent = ms.ToArray();
            ms.Close();

            _ = ImageService.SaveImage(fileContent, Path.Combine("img", layoutRecord.LayoutImage));
        }

        private void AddRow()
        {
            TrackInRaceSim trackInRaceSim = new TrackInRaceSim();
            trackInRaceSim.FieldSelector = Track.TrackInRaceSims.Count + 1;
            TrackInRaceSims.Add(trackInRaceSim);
        }

        private void RemoveRow()
        {
            var rowToDelete = Track.TrackInRaceSims.First(c => c.RaceSimId == SelectedRaceSimId);
            TrackInRaceSims.Remove(rowToDelete);
        }

        private void AddLayoutRow()
        {
            TrackLayout trackLayout = new TrackLayout();
            trackLayout.FieldSelector = TrackLayouts.Count + 1;
            TrackLayouts.Add(trackLayout);
        }

        private async void RemoveLayoutRow()
        {
            var rowToDelete = TrackLayouts.First(c => c.Id == SelectedTrackLayoutId);
            await ImageService.RemoveImage("img", rowToDelete.LayoutImage);
            TrackLayouts.Remove(rowToDelete);
        }
    }
}
