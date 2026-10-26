using GermanTrainer.Data.Configuration;
using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GermanTrainer.Data
{
    public class TrainerDbContext : DbContext
    {
        public TrainerDbContext() { }
        public TrainerDbContext(DbContextOptions<TrainerDbContext> options) : base(options) { }
        public DbSet<GermanNoun> GermanNouns { get; set; }
        public DbSet<GermanAdjective> GermanAdjectives { get; set; }
        public DbSet<GermanVerb> GermanVerbs { get; set; }
        public DbSet<VerbPreposition> VerbPrepositions { get; set; }
        public DbSet<TrainingProgress> TrainingProgress { get; set; }
        public DbSet<SchemaVersion> SchemaVersions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
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
            modelBuilder.Entity<TrainingProgress>(b =>
            {
                b.ToTable("TrainingProgress");
                b.HasKey(p => new { p.Mode, p.ItemKey });
                b.Property(p => p.ItemKey).HasMaxLength(80);
            });
            modelBuilder.Entity<VerbPreposition>(b =>
            {
                b.ToTable("VerbPrepositions");
                b.HasKey(p => p.Id);
                b.Property(p => p.VerbDisplay).HasMaxLength(100);
                b.Property(p => p.Meaning).HasMaxLength(250);
                b.Property(p => p.Preposition).HasMaxLength(20);
                b.HasIndex(p => new { p.GermanVerbId, p.Preposition, p.Case, p.Meaning }).IsUnique();
                b.HasOne(p => p.Verb).WithMany(v => v.PrepositionConstructions)
                    .HasForeignKey(p => p.GermanVerbId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
