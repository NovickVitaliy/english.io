using FluentValidation;
using Learning.Application.DTOs.UserPreferences;

namespace Learning.Application.Validation.UserPreferences;

public abstract class BaseUserPreferencesValidator<T> : AbstractValidator<T> where T : BaseUserPreferencesRequest
{
    protected void ValidateId()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id cannot be empty");
    }

    protected void ValidateUserEmail()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("User email cannot be empty")
            .EmailAddress().WithMessage("User email must be in valid format");
    }

    protected void ValidateNumberOfExampleSentencesPerWord()
    {
        RuleFor(x => x.NumberOfExampleSentencesPerWord)
            .GreaterThan(0).WithMessage("Number of example sentences per word must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("Number of example sentences per word must be less than or equal than 10");
    }

    protected void ValidateDailyWordPracticeLimit()
    {
        RuleFor(x => x.DailyWordPracticeLimit)
            .GreaterThan(0).WithMessage("Number of daily word practice limit must be greater than 0")
            .LessThanOrEqualTo(15).WithMessage("Number of daily word practice limit must be less than or equal than 15");
    }

    protected void ValidateDailySessionsReminderTimes()
    {
        RuleFor(x => x.DailySessionsReminderTimes)
            .NotEmpty().WithMessage("Daily session reminder times must be present")
            .Must(x => x.Length <= 5).WithMessage("Number of daily sessions reminder times must not be greater than 5");
    }
}