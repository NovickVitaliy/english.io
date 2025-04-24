using FluentValidation;
using Learning.Application.DTOs.UserPreferences;
using static Learning.Domain.LocalizationKeys;

namespace Learning.Application.Validation.UserPreferences;

public class CreateUserPreferencesValidator : BaseUserPreferencesValidator<CreateUserPreferencesRequest>
{
    public CreateUserPreferencesValidator()
    {
        ValidateUserEmail();
        ValidateNumberOfExampleSentencesPerWord();
        ValidateDailyWordPracticeLimit();
        ValidateDailySessionsReminderTimes();
        ValidateNotificationChannel();
        RuleFor(x => x.TimezoneId)
            .NotEmpty().WithMessage(TimezoneInfoMustBePresent);
    }
}
