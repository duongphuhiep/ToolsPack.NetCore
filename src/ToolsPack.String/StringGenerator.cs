using System;
using System.Linq;

namespace ToolsPack.String
{
    /// <summary>
    /// Generate a random string
    /// </summary>
    public static class StringGenerator
    {
        private static Random random = new Random();

        /// <summary>
        /// Generate a random string from a given set of characters. The lenght of the random string vari between [minLength..minLenght+lengthVariable]
        /// Note: the "Bogus" library offers the same function: 
        ///  <pre>
        ///     var faker = new Faker();
        ///     var randomString = faker.Random.String2(minLength: 3, maxLength: 10, chars "abcdefghijklmnopqrstuvwxyz"); 
        /// </pre>
        /// </summary>
        /// <param name="minLength">min length of the random string</param>
        /// <param name="lengthVariable">max length = min length + length varaible</param>
        /// <param name="chars">characters set</param>
        /// <returns></returns>
        public static string CreateRandomString(int minLength = 5, int lengthVariable = 0, string chars = "abcdefghijklmnpqrstuvwxyz0123456789")
        {
            if (string.IsNullOrEmpty(chars)) throw new ArgumentNullException(nameof(chars));

            var length = lengthVariable<=0 ? minLength : random.Next(minLength, minLength+lengthVariable);
            if (length < 0) throw new ArgumentException($"Negative length {nameof(minLength)}={minLength} / {nameof(lengthVariable)}={lengthVariable}");

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}