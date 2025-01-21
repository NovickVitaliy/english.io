using Fluxor;

namespace Shared.Store.Theme;

public static class ThemeStateReducers
{
    [ReducerMethod]
    public static ThemeState SetThemeState(ThemeState state, SetThemeStateAction action) => new(action.IsDarkTheme);

    [ReducerMethod]
    public static ThemeState SaveThemeState(ThemeState state, SaveThemeStateAction action) => new(action.IsDarkTheme);
}
