using English.IO.Bot.Database;
using English.IO.Bot.Handlers.Commands.Common;
using English.IO.Bot.Managers.UserStates;
using English.IO.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = English.IO.Bot.Models.User;

namespace English.IO.Bot.Handlers.Commands;

public class StartCommandHandler : ICommandHandler
{
    private readonly EnglishIOBotDbContext _dbContext;
    private readonly IUserStateManager _userStateManager;

    public StartCommandHandler(EnglishIOBotDbContext dbContext, IUserStateManager userStateManager)
    {
        _dbContext = dbContext;
        _userStateManager = userStateManager;
    }

    public bool CanHandle(string command) => command.Equals(Constants.Commands.Start, StringComparison.Ordinal);

    public async Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message?.Chat.Id;
        var user = _dbContext.Users.SingleOrDefault(x => x.TelegramChatId == chatId);
        if (user is {HasSubmittedCode: true})
        {
            await botClient.SendMessage(chatId!, "You have already submitted the code from the web app.", cancellationToken: cancellationToken);
        }
        else
        {
            user = new User()
            {
                HasSubmittedCode = false,
                TelegramChatId = chatId!.Value
            };

            await _dbContext.Users.AddAsync(user, cancellationToken: cancellationToken);
            await _userStateManager.SetStateForUser(chatId.Value, UserState.SubmittingCode);
            await botClient.SendMessage(chatId!, "Submit the code from the ENGLISH.IO web app.", cancellationToken: cancellationToken);
        }
    }
}
