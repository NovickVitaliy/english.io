using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Localization;

namespace Shared.LocalizedDataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class LocalizedRequiredAttribute : BaseLocalizedAttribute
{
    public LocalizedRequiredAttribute(string baseName, string errorName, Type resourceType) 
        : base(baseName, errorName, resourceType)
    {
    }
    
    public bool AllowEmptyStrings { get; set; }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        return AllowEmptyStrings || value is not string stringValue || !string.IsNullOrWhiteSpace(stringValue);
    }
}