using System;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static int AsInt(this string number)
        {
            return Convert.ToInt32(number);
        }

        public static int AsIntOrZero(this string number)
        {
            return string.IsNullOrEmpty(number) ? 0 : number.AsInt();
        }

        public static int? AsIntOrNull(this string number)
        {
            return string.IsNullOrEmpty(number) ? null : number.AsInt();
        }
    }
}
