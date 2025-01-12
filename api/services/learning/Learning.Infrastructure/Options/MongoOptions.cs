using System.ComponentModel.DataAnnotations;

namespace Learning.Infrastructure.Options;

public class MongoOptions
{
    public const string ConfigurationKey = "MongoOptions";
    
    [Required]
    public string ConnectionString { get; init; } = null!;

    [Required]
    public string DatabaseName { get; init; } = null!;
}