using System.ComponentModel.DataAnnotations;

public class RequiredEnumAttribute : RequiredAttribute
{
    public override bool IsValid(object value)
    {
        // First check if it's null (handles if the property is Nullable<T>)
        if (value == null)
        {
            return false;
        }

        // Next, check if the value is a defined member of the enum type
        var type = value.GetType();
        return type.IsEnum && Enum.IsDefined(type, value);
    }
}