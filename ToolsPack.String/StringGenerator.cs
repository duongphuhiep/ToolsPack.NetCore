using System;
using System.Linq;

namespace ToolsPack.String
{
    /// <summary>
    /// Generate a string
    /// </summary>
    public static class StringGenerator
    {
        private static Random random = new Random();

        /// <summary>
        /// Generate a random string with the given length from a given set of characters
        /// </summary>
        /// <param name="length">length of the random string</param>
        /// <param name="chars">characters set</param>
        /// <returns></returns>
        public static string CreateRandomString(int length = 4, string chars = "abcdefghijklmnpqrstuvwxyz0123456789")
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}