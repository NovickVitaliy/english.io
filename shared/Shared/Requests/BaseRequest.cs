namespace Shared.Requests;

public interface IBaseRequest
{
    RequestValidationResult IsValid();
}
