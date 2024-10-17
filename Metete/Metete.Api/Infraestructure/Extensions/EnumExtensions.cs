using System.ComponentModel;
using System.Reflection;

namespace Metete.Api.Infraestructure.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            FieldInfo field = @enum.GetType().GetField(@enum.ToString())!;
            
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            else
            {
                return @enum.ToString();
            }
        }
    }
}
