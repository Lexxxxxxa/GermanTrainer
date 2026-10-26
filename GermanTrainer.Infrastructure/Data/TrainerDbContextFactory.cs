using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Data;

public interface ITrainerDbContextFactory
{
    TrainerDbContext Create();
}

public sealed class TrainerDbContextFactory(DbContextOptions<TrainerDbContext> options) : ITrainerDbContextFactory
{
    public TrainerDbContext Create() => new(options);
}
