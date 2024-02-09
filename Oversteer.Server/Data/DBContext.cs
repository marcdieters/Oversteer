using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = "oversteer.db"
            }
            .ToString();

            var conn = new SqliteConnection(@"Data Source=oversteer.db;");
            conn.Open();
            optionsBuilder.UseSqlite(conn);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        public DbSet<Models.Host> Hosts { get; set; }
        public DbSet<Models.Server> Servers { get; set; }
    }
}
