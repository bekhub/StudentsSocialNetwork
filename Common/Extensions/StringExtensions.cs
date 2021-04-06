using System;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static int AsInt(this string number)
        {
            return Convert.ToInt32(number);
        }
    }
}
