using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;

namespace Shared.LocalizedDataAnnotations;

public class BaseLocalizedAttribute : ValidationAttribute
{
    protected BaseLocalizedAttribute(string baseName, string errorName, Type resourceType)
    {
        var resourceManager = new ResourceManager(baseName, resourceType.Assembly);
        ErrorMessage = resourceManager.GetString(errorName, CultureInfo.CurrentCulture);
    }
}