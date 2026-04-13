using System.Reflection;
using DotNetEnv;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.MessageBus.Events.PracticeNotifications;

namespace Shared.MessageBus;

public static class MessageBusConfiguration
{
    public static IServiceCollection ConfigureRabbitMq(this IServiceCollection services, Assembly? assembly = null)
    {
        Env.TraversePath().Load();

        services.AddScoped<IPracticeNotificationVisitor, PracticeNotificationVisitor>();

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            if (assembly is not null)
            {
                configurator.AddConsumers(assembly);
            }

            configurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(new Uri("amqp://rabbitmq"), hostConfigurator =>
                {
                    hostConfigurator.Username("english.io");
                    hostConfigurator.Password("92D4D403-E935-46C1-9865-AE626EF3BC50");
                });

                factoryConfigurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
