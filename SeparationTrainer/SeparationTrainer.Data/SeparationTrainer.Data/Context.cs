using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) 
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureActivities(modelBuilder);
            ConfigureSessions(modelBuilder);
        }

        protected virtual void ConfigureActivities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(builder => { });
        }

        protected virtual void ConfigureSessions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>(builder =>
            {
                builder.HasMany(session => session.Activities);

            });
        }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Session> Sessions { get; set; }
    }
}
