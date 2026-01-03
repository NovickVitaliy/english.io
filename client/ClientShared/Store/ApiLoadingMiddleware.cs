using Fluxor;
using Shared.Store.Loading;
using Shared.Store.Markers;

namespace Shared.Store;

public class ApiLoadingMiddleware : Middleware
{
    private IDispatcher _dispatcher = null!;
    private IStore _store = null!;

    public override Task InitializeAsync(IDispatcher dispatcher, IStore store)
    {
        _dispatcher = dispatcher;
        _store = store;

        return Task.CompletedTask;
    }

    public override bool MayDispatchAction(object action)
    {
        if (action is IApiAction)
            _dispatcher.Dispatch(new ApiRequestStartedAction());

        return true;
    }

    public override void AfterDispatch(object action)
    {
        if (action is IApiCompletedAction)
            _dispatcher.Dispatch(new ApiRequestFinishedAction());
    }
}
