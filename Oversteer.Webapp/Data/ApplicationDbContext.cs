using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Models.Racing;
using Oversteer.Models.Result;

namespace Oversteer.Webapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admins", NormalizedName = "ADMINS", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        //}

        public DbSet<Country> Countries { get; set; }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarClass> CarClasses { get; set; }

        public DbSet<Dlc> Dlcs { get; set; }

        public DbSet<RaceSim> RaceSims { get; set; }
        public DbSet<CarInRaceSim> CarInRaceSims { get; set; }
        public DbSet<TrackInRaceSim> TrackInRaceSims { get; set; }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackLayout> TrackLayouts { get; set; }


        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueUser> LeagueUsers { get; set; }
        public DbSet<LeagueRegistration> LeagueRegistrations { get; set; }


        public DbSet<Plan> Plans { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureInPlan> FeaturesInPlans { get; set; }
        public DbSet<PlanCategory> PlanCategorys { get; set; }


        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Championship> Championships { get; set; }
        public DbSet<Race> Races { get; set; }

        public DbSet<Result> Results { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Lap> Laps { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Models.Host> Hosts { get; set; }
        public DbSet<Server> Servers { get; set; }
    }
}