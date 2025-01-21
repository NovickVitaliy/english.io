
using Fluxor;

namespace Shared.Store.Theme;

[FeatureState]
public record ThemeState(bool IsDarkTheme)
{
    private ThemeState() : this(false)
    {

    }
}
