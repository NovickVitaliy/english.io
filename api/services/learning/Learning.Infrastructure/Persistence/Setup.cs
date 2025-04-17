using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Learning.Infrastructure.Persistence;

public static class Setup
{
    public static Task InitializeDatabase(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SchedulingDatabase");
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Setup).Assembly)
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            upgrader.PerformUpgrade();
        }

        return Task.CompletedTask;
    }
}
