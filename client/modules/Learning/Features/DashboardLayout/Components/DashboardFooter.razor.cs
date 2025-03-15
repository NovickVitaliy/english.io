using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Shared.Extensions;

namespace Learning.Features.DashboardLayout.Components;

public partial class DashboardFooter : ComponentBase, IDisposable
{
    private bool _disposed;
    [Inject] private IStringLocalizer<DashboardFooter> Localizer { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    private string Location { get; set; } = null!;

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
        Location = NavigationManager.GetRelativePath();
        StateHasChanged();
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Location = NavigationManager.GetRelativePath();
        StateHasChanged();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                NavigationManager.LocationChanged -= OnLocationChanged;
            }

            _disposed = true;
        }
    }

    ~DashboardFooter()
    {
        Dispose(false);
    }
}

