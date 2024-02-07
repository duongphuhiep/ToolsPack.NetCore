using System;
using System.Collections.Generic;
using System.Text;

namespace ToolsPack.String
{
    public static class ShortGuid
    {
        /// <summary>
        /// Convert a GUID (32-lenght) to a shorter 22-length string. 
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
    }
}
