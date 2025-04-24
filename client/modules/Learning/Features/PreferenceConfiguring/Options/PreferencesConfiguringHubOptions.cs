using System.ComponentModel.DataAnnotations;

namespace Learning.Features.PreferenceConfiguring.Options;

public class PreferencesConfiguringHubOptions
{
    public const string Key = "PreferencesConfiguringHubOptions";

    [Required]
    public string HubUrl { get; init; } = null!;
}
