using GermanTrainer.Data.Configuration;
using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GermanTrainer.Data
{
    public class TrainerDbContext : DbContext
    {
        public DbSet<GermanNoun> GermanNouns { get; set; }
        public DbSet<GermanAdjective> GermanAdjectives { get; set; }
        public DbSet<GermanVerb> GermanVerbs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GermanNounConfiguration());
            modelBuilder.ApplyConfiguration(new GermanAdjectiveConfiguration());
            modelBuilder.ApplyConfiguration(new GermanVerbConfiguration());
        }
    }
}