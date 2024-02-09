using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Plans
{
    public partial class _UpsertFeature
    {
        [Inject]
        public IPlanService PlanService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Feature Feature { get; set; } = new Feature();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public async Task ShowAsync(Feature feature)
        {
            Feature = feature;
            ShowDialog = true;
            StateHasChanged();
        }

        public async Task Close()
        {
            ShowDialog = false;
            ShowLoader = false;

            StateHasChanged();
            await CloseEventCallback.InvokeAsync(true);
        }

        public async Task Remove()
        {
            try
            {
                await PlanService.RemoveFeature(Feature.Id);
                var confirm = await Swal.ShowInfoWithConfirmOk("Feature removed", "This feature has been removed succesfully.");
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

        protected async void HandleValidSubmit()
        {
            try
            {
                ShowLoader = true;
                await PlanService.UpsertFeature(Feature);

                ShowLoader = false;
                StateHasChanged();

                var confirm = await Swal.ShowInfoWithConfirmOk("Category saved", "This category has been saved succesfully.");
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
