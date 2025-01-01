using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Localization;

namespace Shared.LocalizedDataAnnotations;

public class LocalizedRequiredAttribute : RequiredAttribute
{
    public LocalizedRequiredAttribute(string baseName, string errorName, Type resourceType) 
    {
        var resourceManager = new ResourceManager(baseName, resourceType.Assembly);
        ErrorMessage = resourceManager.GetString(errorName, CultureInfo.CurrentCulture);
    }
}