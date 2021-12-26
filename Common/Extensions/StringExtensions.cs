using System;
using System.IO;

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
            return !string.IsNullOrEmpty(number) && int.TryParse(number, out var parsed) ? parsed : 0;
        }

        public static int? AsIntOrNull(this string number)
        {
            return !string.IsNullOrEmpty(number) && int.TryParse(number, out var parsed) ? parsed : null;
        }
        
        public static string GeneratePictureName(this string pictureName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(pictureName)}";
        }
    }
}
