
using Fluxor;
using Refit;
using Shared.Extensions;

namespace Shared.Store;

public abstract class BaseEffects
{
    protected static async Task ProcessRefitApiRequest<TApiResponse, TSuccessAction, TFailureAction>(
        Func<Task<TApiResponse>> apiCall,
        Func<TApiResponse, TSuccessAction> successActionFactory,
        Func<string, TFailureAction> failureActionFactory,
        IDispatcher dispatcher)
    {
        try
        {
            var apiResponse = await apiCall();
            var successAction = successActionFactory(apiResponse);
            dispatcher.Dispatch(successAction);
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            var failureAction = failureActionFactory(problemDetails.Detail ?? "Error_Occured");
            dispatcher.Dispatch(failureAction);
        }
    }

    protected static async Task ProcessRefitApiRequestWithNoResponse<TSuccessAction, TFailureAction>(
        Func<Task> apiCall,
        Func<TSuccessAction> successActionFactory,
        Func<string, TFailureAction> failureActionFactory,
        IDispatcher dispatcher)
    {
        try
        {
            await apiCall();
            var successAction = successActionFactory();
            dispatcher.Dispatch(successAction);
        }
        catch (ApiException e)
        {
            ProcessException(e, dispatcher, failureActionFactory);
        }
    }

    private static void ProcessException<TFailureAction>(ApiException exception, IDispatcher dispatcher, Func<string, TFailureAction> failureActionFactory)
    {
        var problemDetails = exception.ToProblemDetails();
        var failureAction = failureActionFactory(problemDetails.Detail ?? "Error_Occured");
        dispatcher.Dispatch(failureAction);
    }
}
