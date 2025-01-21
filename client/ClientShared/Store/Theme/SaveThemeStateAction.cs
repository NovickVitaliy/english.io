
namespace Shared.Store.Theme;

public record SaveThemeStateAction(bool IsDarkTheme)
{
    private SaveThemeStateAction() : this(false)
    {

    }
}
