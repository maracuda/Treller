using System;

namespace ViskeyTube.Common
{
    public static class StringExtensions
    {
        public static string SafeSubString(this string src, int startIndex, int length)
        {
            return src.Substring(0, Math.Min(length, src.Length));
        }
    }
}