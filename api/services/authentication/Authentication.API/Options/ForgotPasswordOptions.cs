namespace Authentication.API.Options;

public class ForgotPasswordOptions
{
    public const string ConfigurationKey = "ForgotPasswordOptions";

    public string MessageTemplate { get; init; } = null!;
}
