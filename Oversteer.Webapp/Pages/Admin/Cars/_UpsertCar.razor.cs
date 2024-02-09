using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Oversteer.Webapp.Services;
using Oversteer.Models;

namespace Oversteer.Webapp.Pages.Admin.Cars
{
    public partial class _UpsertCar
    {
        [Inject]
        public ICarService CarService { get; set; }
        [Inject]
        public IDLCService DLCService { get; set; }
        [Inject]
        public IRaceSimService RaceSimService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Models.Car Car { get; set; } = new Models.Car();

        public List<CarBrand> CarBrands { get; set; } = new List<CarBrand>();
        public List<Dlc> Dlcs { get; set; } = new List<Dlc>();
        public List<RaceSim> RaceSims { get; set; } = new List<RaceSim>();
        public List<CarClass> CarClasses { get; set; } = new List<CarClass>();

        public IReadOnlyList<IBrowserFile> SelectedFiles { get; set; }
        public Guid SelectedCarInRaceSimId { get; set; }

        public List<CarInRaceSim> CarInRaceSims { get; set; } = new List<CarInRaceSim>();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public string FileMessage { get; set; }
        public string CarInSimMessage { get; set; }
        protected bool HideAccEntries { get; set; } = true;

        public async Task ShowAsync(Models.Car car)
        {
            Car = car;
            CarBrands = await CarService.GetCarBrands();
            RaceSims = await RaceSimService.GetAllRaceSims();
            Dlcs = await DLCService.GetDLCs();
            CarClasses = await CarService.GetCarClasses();

            if (Car.Id != Guid.Empty)
            {
                CarInRaceSims = RaceSimService.GetAllCarsInRaceSim(Car.Id);
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

        protected async void HandleValidSubmit()
        {
            try
            {
                if (SelectedFiles != null && CarInRaceSims.Count > 0)
                {
                    ShowLoader = true;

                    string ext = Path.GetExtension(SelectedFiles[0].Name);
                    Car.Image = Guid.NewGuid().ToString() + ext;

                    Stream stream = SelectedFiles[0].OpenReadStream();
                    MemoryStream ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    stream.Close();

                    byte[] fileContent = ms.ToArray();
                    ms.Close();

                    Guid carid = await CarService.UpsertCar(Car, fileContent);
                    foreach(var carInSim in CarInRaceSims)
                    {
                        carInSim.CarId = carid;
                        await RaceSimService.SaveCarInSim(carInSim);
                    }

                    ShowLoader = false;
                    StateHasChanged();

                    var confirm = await Swal.ShowInfoWithConfirmOk("Car saved", "This car has been saved succesfully.");
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

                    if (CarInRaceSims.Count == 0)
                    {
                        CarInSimMessage = "You need to specify at least one race sim.";
                    }
                    else
                    {
                        CarInSimMessage = "";
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

        private void AddRow()
        {
            CarInRaceSim carInRaceSim = new CarInRaceSim();
            CarInRaceSims.Add(carInRaceSim);
        }

        private void RemoveRow()
        {
            var rowToDelete = CarInRaceSims.First(c => c.Id == SelectedCarInRaceSimId);
            CarInRaceSims.Remove(rowToDelete);
        }

        protected void RaceSimSelected(CarInRaceSim carInRaceSim)
        {
            HideAccEntries = true;
            var selRaceSim = RaceSims.First(r => r.Id == carInRaceSim.RaceSimId);

            switch (selRaceSim.Name)
            {
                case "Assetto Corsa Compitizione":
                    HideAccEntries = false;
                    break;
            }
        }

        protected void ContentTypeSelected(CarInRaceSim carInRaceSim)
        {
            if (carInRaceSim.ContentType == ContentType.Base)
            {
                carInRaceSim.DisableDlc = true;
            }
            else
            {
                carInRaceSim.DisableDlc = false;
            }
        }
    }
}
