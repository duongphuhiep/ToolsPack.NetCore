using System;

namespace ToolsPack.String
{
    /// <summary>
    /// ShortGuid - a random length-22 string with base64 characters. It can be converted back to Guid.
    /// </summary>
    public static class ShortGuid
    {
        /// <summary>
        /// Convert a GUID (32-length) to a shorter 22-length string. 
        /// if urlFriendly = true then Replace the '+' and '/' with '-' and '_'
        /// </summary>
        /// <param name="newGuid"></param>
        /// <param name="needUrlFriendly">if true: Replace the '+' and '/' with '-' and '_'</param>
        /// <returns></returns>
        public static string ToShortGuid(this Guid newGuid, bool needUrlFriendly = false)
        {
            string modifiedBase64 = Convert.ToBase64String(newGuid.ToByteArray(), Base64FormattingOptions.None)
                .Substring(0, 22);
            if (needUrlFriendly)
            {
                return modifiedBase64.Replace('+', '-').Replace('/', '_'); // avoid invalid URL characters
            }
            return modifiedBase64;
        }

        /// <summary>
        /// Convert base64 Short Guid back to normal Guid
        /// </summary>
        /// <param name="shortGuid">a length-22 string</param>
        /// <param name="isUrlFriendly">if true: Replace the '-' and '_' with  '+' and '/'</param>
        /// <returns></returns>
        public static Guid Parse(string shortGuid, bool isUrlFriendly = false)
        {
            if (shortGuid == null) throw new ArgumentNullException(nameof(shortGuid));
            if (shortGuid.Length != 22) throw new ArgumentException($"invalid {nameof(shortGuid)} length <> 22");
            string base64 = (isUrlFriendly ? shortGuid.Replace('-', '+').Replace('_', '/') : shortGuid) + "==";
            byte[] bytes = Convert.FromBase64String(base64);
            return new Guid(bytes);
        }

        private static readonly char[] _Base64UrlFriendlyChars = 
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".ToCharArray();
        private static readonly char[] _Base64Chars = 
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();
        
        /// <summary>
        /// Generate a ShortGuid - a random length-22 string with base64 characters.
        /// </summary>
        /// <returns>a random length-22 string</returns>
        public static string New(bool isUrlFriendly = false) => CreateNewShortGuid(isUrlFriendly ? _Base64UrlFriendlyChars : _Base64Chars);

        private static string CreateNewShortGuid(char[] possibleChars)
        {
#if NET8_0_OR_GREATER
            Span<byte> randomBytes = stackalloc byte[22];
            Random.Shared.NextBytes(randomBytes);
            return string.Create(22, randomBytes.ToArray(), (chars, bytes) =>
            {
                for (int i = 0; i < 22; i++)
                {
                    chars[i] = possibleChars[bytes[i]%64];
                }
            });
#else
            var result = new char[22];
            for (int i = 0; i < 22; i++)
            {
                result[i] = possibleChars[StringGenerator.random.Next(64)];
            }
            return new string(result);
#endif
        }
    }
}
