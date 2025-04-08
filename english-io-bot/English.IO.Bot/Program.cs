// See https://aka.ms/new-console-template for more information

using English.IO.Bot.BackgroundTasks;
using English.IO.Bot.Exceptions;
using English.IO.Bot.Handlers;
using English.IO.Bot.Handlers.Callbacks.Common;
using English.IO.Bot.Handlers.Commands.Common;
using English.IO.Bot.Handlers.States.Common;
using English.IO.Bot.Managers.UserStates;
using English.IO.Bot.Providers.States.UserStates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

await Host.CreateDefaultBuilder()
    .UseSerilog()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json", optional: false);
        builder.AddUserSecrets<Program>();
    })
    .ConfigureServices((context, services) =>
    {
        var botToken = context.Configuration["TelegramBotToken"] ?? throw new TelegramBotTokenNotFoundException();
        services.AddSingleton<IUserStateProvider, InMemoryCacheUserStateProvider>();
        services.AddSingleton<IUserStateManager, UserStateManager>();
        services.AddMemoryCache();
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));
        services.AddSingleton<UpdateHandler>();
        services.AddHostedService<BotHostedService>();

        services.Scan(scan => scan.FromAssemblyOf<IUserStateHandler>()
            .AddClasses(classes => classes.AssignableTo<IUserStateHandler>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(scan => scan.FromAssemblyOf<ICommandHandler>()
            .AddClasses(classes => classes.AssignableTo<ICommandHandler>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        services.Scan(x => x.FromAssemblyOf<ICallbackHandler>()
            .AddClasses(classes => classes.AssignableTo<ICallbackHandler>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }).RunConsoleAsync();
