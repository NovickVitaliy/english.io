using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}