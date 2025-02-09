using System.Text.Json;

namespace Shared;

public static class ClientConstants
{
    public const string UserDataKey = "X-USER";
    public const string ThemeKey = "X-IS_DARK_THEME";

    public static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };
}
