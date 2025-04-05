namespace Shared.Services.Contracts;

public interface ICurrentUserAccessor
{
    string? GetEmail();
}
