using Microsoft.AspNetCore.Components;

namespace Shared.Extensions;

public static class NavigationManagerExtensions
{
    public static string GetRelativePath(this NavigationManager navigationManager)
    {
        return navigationManager.Uri[(navigationManager.BaseUri.Length - 1)..];
    }
}
