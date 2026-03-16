using FluentValidation.TestHelper;
using MimeKit.Text;
using Notifications.API.DTOs;
using Notifications.API.Validators;

namespace Notifications.API.Tests;

public class SendEmailMessageRequestValidatorTests
{
    private readonly SendEmailMessageRequestValidator _validator = new();

    private static SendEmailMessageRequest ValidRequest() => new("user@example.com", "John Doe", "Hello", TextFormat.Plain, "This is the email body.");

    [Fact]
    public void ReceiverEmail_Empty_ShouldHaveValidationError()
    {
        var request = ValidRequest() with { ReceiverEmail = "" };
        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(x => x.ReceiverEmail)
            .WithErrorMessage("Receiver Email must not be empty");
    }

    [Fact]
    public void ReceiverEmail_InvalidFormat_ShouldHaveValidationError()
    {
        var request = ValidRequest() with { ReceiverEmail = "not-an-email" };
        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(x => x.ReceiverEmail)
            .WithErrorMessage("Receiver Email must be in valid format");
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name+tag@domain.co.uk")]
    [InlineData("user123@sub.domain.org")]
    public void ReceiverEmail_ValidFormat_ShouldNotHaveValidationError(string email)
    {
        var request = ValidRequest() with { ReceiverEmail = email };
        _validator.TestValidate(request)
            .ShouldNotHaveValidationErrorFor(x => x.ReceiverEmail);
    }

    [Fact]
    public void ReceiverName_Empty_ShouldHaveValidationError()
    {
        var request = ValidRequest() with { ReceiverName = "" };
        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(x => x.ReceiverName)
            .WithErrorMessage("Receiver Name cannot be empty");
    }

    [Fact]
    public void ReceiverName_Valid_ShouldNotHaveValidationError()
    {
        var request = ValidRequest() with { ReceiverName = "Jane Doe" };
        _validator.TestValidate(request)
            .ShouldNotHaveValidationErrorFor(x => x.ReceiverName);
    }

    [Fact]
    public void Subject_Empty_ShouldHaveValidationError()
    {
        var request = ValidRequest() with { Subject = "" };
        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(x => x.Subject)
            .WithErrorMessage("Subject of email cannot be empty");
    }

    [Fact]
    public void Subject_Valid_ShouldNotHaveValidationError()
    {
        var request = ValidRequest() with { Subject = "Monthly Report" };
        _validator.TestValidate(request)
            .ShouldNotHaveValidationErrorFor(x => x.Subject);
    }

    [Fact]
    public void TextFormat_Empty_ShouldHaveValidationError()
    {
        var request = ValidRequest() with { TextFormat = null };
        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(x => x.TextFormat)
            .WithErrorMessage("Text Format cannot be empty");
    }

    [Fact]
    public void TextFormat_Valid_ShouldNotHaveValidationError()
    {
        var request = ValidRequest() with { TextFormat = TextFormat.Html };
        _validator.TestValidate(request)
            .ShouldNotHaveValidationErrorFor(x => x.TextFormat);
    }

    [Fact]
    public void Body_Empty_ShouldHaveValidationError()
    {
        var request = ValidRequest() with { Body = "" };
        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(x => x.Body)
            .WithErrorMessage("Body of the message cannot be empty");
    }

    [Fact]
    public void Body_Valid_ShouldNotHaveValidationError()
    {
        var request = ValidRequest() with { Body = "Dear John, ..." };
        _validator.TestValidate(request)
            .ShouldNotHaveValidationErrorFor(x => x.Body);
    }

    // --- Full request ---

    [Fact]
    public void AllFieldsValid_ShouldPassValidation()
    {
        _validator.TestValidate(ValidRequest())
            .ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AllFieldsEmpty_ShouldHaveValidationErrorForAllFields()
    {
        var request = new SendEmailMessageRequest("", "", "", null, "");

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ReceiverEmail);
        result.ShouldHaveValidationErrorFor(x => x.ReceiverName);
        result.ShouldHaveValidationErrorFor(x => x.Subject);
        result.ShouldHaveValidationErrorFor(x => x.TextFormat);
        result.ShouldHaveValidationErrorFor(x => x.Body);
    }
}
