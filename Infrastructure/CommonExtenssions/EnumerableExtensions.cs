using System;
using System.ComponentModel;

namespace SKBKontur.Infrastructure.CommonExtenssions
{
    public static class EnumerableExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}