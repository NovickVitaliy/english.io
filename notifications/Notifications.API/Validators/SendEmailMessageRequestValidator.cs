using FluentValidation;
using Notifications.API.DTOs;

namespace Notifications.API.Validators;

public class SendEmailMessageRequestValidator : AbstractValidator<SendEmailMessageRequest>
{
    public SendEmailMessageRequestValidator()
    {
        RuleFor(x => x.ReceiverEmail)
            .NotEmpty().WithMessage("Receiver Email must not be empty")
            .EmailAddress().WithMessage("Receiver Email must be in valid format");

        RuleFor(x => x.ReceiverName)
            .NotEmpty().WithMessage("Receiver Name cannot be empty");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject of email cannot be empty");

        RuleFor(x => x.TextFormat)
            .NotEmpty().WithMessage("Text Format cannot be empty");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body of the message cannot be empty");
    }
}
