using System.ComponentModel;
using System.Reflection;

namespace GSRU_API.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString<T>(this T enumValue)
        {
            ArgumentNullException.ThrowIfNull(enumValue);
            string valueName = enumValue.ToString() ?? string.Empty;
            Type type = enumValue.GetType();
            FieldInfo field = type.GetField(valueName) 
                ?? throw new ArgumentException("Enum value not found");

            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
