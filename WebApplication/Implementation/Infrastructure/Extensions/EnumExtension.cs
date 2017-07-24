using System;
using System.ComponentModel;

namespace WebApplication.Implementation.Infrastructure.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}