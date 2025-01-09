namespace Shared.Requests;

public abstract record BaseRequest
{
    public abstract RequestValidationResult IsValid();
}