using Microsoft.AspNetCore.Components;
using Org.BouncyCastle.Bcpg.Sig;
using Oversteer.Models;
using Oversteer.Webapp.Services;

namespace Oversteer.Webapp.Pages.Admin.Plans
{
    public partial class _UpsertPlan
    {
        [Inject]
        public IPlanService PlanService { get; set; }
        [Inject]
        public ISwalService Swal { get; set; }


        public _UpsertCategory? _UpsertCategory { get; set; }
        public _UpsertFeature? _UpsertFeature { get; set; }

        public PlanCategory SelPlanCategory { get; set; } = new PlanCategory();
        public Feature SelFeature { get; set; } = new Feature();
        public FeatureInPlan SelFeatureInPlan { get; set; } = new FeatureInPlan();
        public List<PlanCategory> PlanCategories { get; set; } = new List<PlanCategory>();
        public List<Feature> Features { get; set; } = new List<Feature>();
        public bool ShowDialog { get; set; } = false;
        public bool ShowLoader { get; set; } = false;
        public Plan Plan { get; set; } = new Plan();
        public int SelectedFeatureInPlan { get; set; }

        public string CategoryButtonText { get; set; } = "Add Category";
        public string FeatureButtonText { get; set; } = "Add Feature";

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public async Task ShowAsync(Plan plan)
        {
            Plan = plan;
            PlanCategories = await PlanService.GetCategories();
            Features = await PlanService.GetFeatures();

            if (plan.Id == Guid.Empty)
            {
                Plan.Features.Add(SelFeatureInPlan);
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

        protected async Task AddCategory()
        {
            await _UpsertCategory!.ShowAsync(SelPlanCategory);   
        }

        protected async Task UpsertCategoryDialog_OnDialogClose()
        {
            PlanCategories = await PlanService.GetCategories();
            SelPlanCategory = new PlanCategory();
        }

        protected async Task AddFeature()
        {
            await _UpsertFeature!.ShowAsync(SelFeature);
        }

        protected async Task UpsertFeatureDialog_OnDialogClose()
        {
            Features = await PlanService.GetFeatures();
            SelFeatureInPlan = new FeatureInPlan();
        }

        protected async void HandleValidSubmit()
        {
            try
            {
                ShowLoader = true;

                if (Plan.Features.Count > 0)
                {
                    foreach (var feature in Plan.Features)
                    {
                        if (feature.FeatureId != Guid.Empty && feature.CategoryId != Guid.Empty)
                        {
                            feature.Feature = Features.First(f => f.Id == feature.FeatureId);
                            feature.PlanCategory = PlanCategories.First(f => f.Id == feature.CategoryId);
                        }
                        else
                        {
                            await Swal.ShowError($"You haven't filled in the form properly");
                            return;
                        }
                    }

                    await PlanService.UpsertPlan(Plan);

                    ShowLoader = false;
                    StateHasChanged();

                    var confirm = await Swal.ShowInfoWithConfirmOk("Plan saved", "This plan has been saved succesfully.");
                    if (confirm.IsConfirmed)
                    {
                        ShowDialog = false;
                        await CloseEventCallback.InvokeAsync(true);
                    }
                }
                else
                {
                    await Swal.ShowError($"You haven't filled in the form properly");
                }
            }
            catch (Exception ex)
            {
                ShowLoader = false;
                StateHasChanged();
                await Swal.ShowError($"That didn't work. Error: {ex.Message}");
            }
        }

        private void AddRow()
        {
            FeatureInPlan featureInPlan = new FeatureInPlan();
            featureInPlan.FieldSelector = Plan.Features.Count + 1;
            Plan.Features.Add(featureInPlan);
        }

        private void RemoveRow()
        {
            if (SelectedFeatureInPlan > 0)
            {
                var rowToDelete = Plan.Features.First(c => c.FieldSelector == SelectedFeatureInPlan);
                Plan.Features.Remove(rowToDelete);
                SelectedFeatureInPlan = 0;
            }
        }
    }
}
