using FluentValidation;
using Learning.Application.DTOs.UserPreferences;
using static Learning.Domain.LocalizationKeys;
namespace Learning.Application.Validation.UserPreferences;

public abstract class BaseUserPreferencesValidator<T> : AbstractValidator<T> where T : IBaseUserPreferencesRequest
{
    protected void ValidateId()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(IdCannotBeEmpty);
    }

    protected void ValidateUserEmail()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage(UserEmailCannotBeEmpty)
            .EmailAddress().WithMessage(UserEmailMustBeInValidFormat);
    }

    protected void ValidateNumberOfExampleSentencesPerWord()
    {
        RuleFor(x => x.NumberOfExampleSentencesPerWord)
            .GreaterThan(0).WithMessage(NumberOfExampleSentencesPerWordMustBeGreaterThanZero)
            .LessThanOrEqualTo(10).WithMessage(NumberOfExampleSentencesPerWordMustBeLessOrEqualToTen);
    }

    protected void ValidateDailyWordPracticeLimit()
    {
        RuleFor(x => x.DailyWordPracticeLimit)
            .GreaterThan(0).WithMessage(NumberOfDailyWordPracticeLimitMustBeGreaterThanZero)
            .LessThanOrEqualTo(15).WithMessage(NumberOfDailyWordPracticeLimitMustBeLessThanOrEqualTo15);
    }

    protected void ValidateDailySessionsReminderTimes()
    {
        RuleFor(x => x.DailySessionsReminderTimes)
            .NotEmpty().WithMessage(DailySessionsReminderTimesMustBePresent)
            .Must(x => x is { Length: <= 5 }).WithMessage(NumberOfDailySessionsReminderTimesMustNotBeGreatedThan5);
    }

    protected void ValidateNotificationChannel()
    {
        RuleFor(x => x.NotificationChannel)
            .IsInEnum();
    }
}
