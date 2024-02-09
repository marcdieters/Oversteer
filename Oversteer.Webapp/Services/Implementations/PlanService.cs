using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.Sig;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using System.Diagnostics.Metrics;

namespace Oversteer.Webapp.Services
{
    public class PlanService : IPlanService
    {
        private readonly ApplicationDbContext _db;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public PlanService(ApplicationDbContext db, AuthenticationStateProvider authenticationStateProvider)
        {
            _db = db;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public Task<List<Plan>> GetPlans()
        {
            List<Plan> plans = _db.Plans.Include(p => p.Features).OrderBy(c => c.Name).ToList();
            return Task.FromResult(plans);
        }

        public Task RemovePlan(Plan plan)
        {
            throw new NotImplementedException();
        }

        public Task UpsertPlan(Plan plan)
        {
            if (!_db.Plans.Any(c => c.Id == plan.Id))
            {
                _db.Plans.Add(plan);
            }
            else
            {
                _db.Entry(plan).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<PlanCategory>> GetCategories()
        {
            var categories = _db.PlanCategorys.OrderBy(p => p.Name).ToList();
            return Task.FromResult(categories);
        }

        public Task UpsertCategory(PlanCategory category)
        {
            if (!_db.PlanCategorys.Any(c => c.Id == category.Id))
            {
                _db.PlanCategorys.Add(category);
            }
            else
            {
                _db.Entry(category).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return Task.CompletedTask;
        }

        public Task RemoveCategory(Guid categoryId)
        {
            if (_db.PlanCategorys.Any(c => c.Id == categoryId))
            {
                var category = _db.PlanCategorys.First(c => c.Id == categoryId);
                _db.PlanCategorys.Remove(category);
                _db.SaveChanges();
            }

            return Task.CompletedTask;
        }

        public Task<List<Feature>> GetFeatures()
        {
            var features = _db.Features.OrderBy(p => p.Name).ToList();
            return Task.FromResult(features);
        }

        public Task UpsertFeature(Feature feature)
        {
            if (!_db.Features.Any(c => c.Id == feature.Id))
            {
                _db.Features.Add(feature);
            }
            else
            {
                _db.Entry(feature).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return Task.CompletedTask;
        }

        public Task RemoveFeature(Guid featureId)
        {
            if (_db.Features.Any(c => c.Id == featureId))
            {
                var feature = _db.Features.First(c => c.Id == featureId);
                _db.Features.Remove(feature);
                _db.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
