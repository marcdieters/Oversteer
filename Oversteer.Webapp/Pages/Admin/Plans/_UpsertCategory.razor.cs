using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Plans
{
    public partial class _UpsertCategory
    {
        [Inject]
        public IPlanService PlanService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public PlanCategory Category { get; set; } = new PlanCategory();

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public async Task ShowAsync(PlanCategory category)
        {
            Category = category;
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
                await PlanService.RemoveCategory(Category.Id);
                var confirm = await Swal.ShowInfoWithConfirmOk("Category removed", "This category has been removed succesfully.");
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
                await PlanService.UpsertCategory(Category);

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
