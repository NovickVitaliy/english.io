using Learning.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Database.Statistics;

public class StatisticsDbContext : DbContext
{
    public DbSet<UserStatisticsSnapshot> UserStatisticsSnapshots => Set<UserStatisticsSnapshot>();
    public DbSet<UserStreak> UserStreaks => Set<UserStreak>();
    public DbSet<WordProgressHistory> WordProgressHistories => Set<WordProgressHistory>();

    public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StatisticsDbContext).Assembly);
    }
}
