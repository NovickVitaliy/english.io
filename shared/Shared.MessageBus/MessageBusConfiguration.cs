using System.Reflection;
using DotNetEnv;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.MessageBus;

public static class MessageBusConfiguration
{
    public static IServiceCollection ConfigureRabbitMq(this IServiceCollection services, Assembly? assembly = null)
    {
        Env.TraversePath().Load();
        
        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            if (assembly is not null)
            {
                configurator.AddConsumers(assembly);
            }
            
            configurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(new Uri(Env.GetString("MESSAGE_BROKER_HOST")), hostConfigurator =>
                {
                    hostConfigurator.Username(Env.GetString("MESSAGE_BROKER_USERNAME"));
                    hostConfigurator.Password(Env.GetString("MESSAGE_BROKER_PASSWORD"));
                });
                
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}