using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface IPlanService
    {
        Task<List<Plan>> GetPlans();
        Task UpsertPlan (Plan plan);
        Task RemovePlan (Plan plan);

        Task<List<PlanCategory>> GetCategories ();
        Task UpsertCategory (PlanCategory plan);
        Task RemoveCategory(Guid categoryId);

        Task<List<Feature>> GetFeatures();
        Task UpsertFeature(Feature feature);
        Task RemoveFeature(Guid featureId);
    }
}
