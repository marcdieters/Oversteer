using Microsoft.AspNetCore.Components;
using Oversteer.Models;
using Oversteer.Webapp.Pages.Admin.Country;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Plans
{
    public partial class Index
    {
        [Inject]
        public IPlanService PlanService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }

        protected _UpsertPlan? _UpsertPlan { get; set; }
        public List<Plan> Plans { get; set; } = new List<Plan>();
        public Plan SelectedPlan { get; set; } = new Plan();
        public bool ShowDialog { get; set; }
        public bool ShowLoader { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                ShowLoader = true;
                Plans = await PlanService.GetPlans();
                ShowLoader = false;
            }
            catch (Exception ex)
            {
                Plans = new List<Plan>();
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected void ShowOptions(Plan plan)
        {
            SelectedPlan = plan;
            ShowDialog = true;
            StateHasChanged();
        }

        protected async Task UpsertPlan(bool newrecord)
        {
            if (newrecord)
            {
                SelectedPlan = new Plan();
            }

            await _UpsertPlan!.ShowAsync(SelectedPlan);
        }

        protected async Task UpsertPlanDialog_OnDialogClose()
        {
            try
            {
                Plans = await PlanService.GetPlans();
            }
            catch (Exception ex)
            {
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        protected async Task RemovePlan()
        {
            try
            {
                var confirm = await Swal.ShowInfoWithConfirm("Remove country?", "Are you sure you want to remove this country?");

                if (confirm.IsConfirmed)
                {
                    await PlanService.RemovePlan(SelectedPlan);
                    Plans.Remove(SelectedPlan);
                    ShowDialog = false;
                }
            }
            catch (Exception ex)
            {
                Plans = new List<Plan>();
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
