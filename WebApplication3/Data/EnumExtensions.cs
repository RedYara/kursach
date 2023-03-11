using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApplication3.Data.Initializers;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public static class EnumExtensions
    {
        public static string GetDisplayValue(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return value.ToString();
            }
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes == null)
            {
                return value.ToString();
            }
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
        public static string GetDisplayShortName(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return value.ToString();
            }
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes == null)
            {
                return value.ToString();
            }
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].ShortName : value.ToString();
        }
        public static string GetDescriptionValue(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return value.ToString();
            }
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (descriptionAttributes == null)
            {
                return value.ToString();
            }
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Description : value.ToString();
        }
        public static List<Role> GetAllowedRoles(this Enum value)
        {
            var result = new List<Role>();
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return result;
            }
            var allowedRoleAttributes = fieldInfo.GetCustomAttributes(
                typeof(AllowedForRoleAttribute), false) as AllowedForRoleAttribute[];
            if (allowedRoleAttributes != null && allowedRoleAttributes.Length > 0)
            {
                return allowedRoleAttributes[0].Roles;
            }
            return result;
        }
    }
}