namespace Shared.LocalizedDataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class LocalizedEmailAddressAttribute : BaseLocalizedAttribute
{
    public LocalizedEmailAddressAttribute(string baseName, string errorName, Type resourceType) 
        : base(baseName, errorName, resourceType)
    {
    }
    
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (!(value is string valueAsString))
        {
            return false;
        }

        int index = valueAsString.IndexOf('@');

        return
            index > 0 &&
            index != valueAsString.Length - 1 &&
            index == valueAsString.LastIndexOf('@');
    }
}