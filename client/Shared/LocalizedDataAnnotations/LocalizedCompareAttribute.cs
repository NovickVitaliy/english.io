using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;

namespace Shared.LocalizedDataAnnotations;

public class LocalizedCompareAttribute : CompareAttribute
{
    public LocalizedCompareAttribute(string baseName, string errorName, Type resourceType, string otherProperty) : base(otherProperty)
    {
        var resourceManager = new ResourceManager(baseName, resourceType.Assembly);
        ErrorMessage = resourceManager.GetString(errorName, CultureInfo.CurrentCulture);
    }
}