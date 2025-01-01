using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;

namespace Shared.LocalizedDataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class LocalizedEmailAddressAttribute : ValidationAttribute
{
    private readonly EmailAddressAttribute _innerValidator;
    public LocalizedEmailAddressAttribute(string baseName, string errorName, Type resourceType) 
    {
        var resourceManager = new ResourceManager(baseName, resourceType.Assembly);
        ErrorMessage = resourceManager.GetString(errorName, CultureInfo.CurrentCulture);
        _innerValidator = new EmailAddressAttribute();
    }

    public override bool IsValid(object? value)
    {
        return _innerValidator.IsValid(value);
    }
}