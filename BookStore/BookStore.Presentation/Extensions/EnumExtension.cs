using System.ComponentModel;

namespace BookStore.Presentation.Extensions;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        System.Reflection.FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field is null)
        {
            return value.ToString();
        }

        DescriptionAttribute? attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute?.Description ?? value.ToString();
    }
}