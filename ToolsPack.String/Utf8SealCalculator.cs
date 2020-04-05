using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ToolsPack.String
{
    /// <summary>
    /// Compute the signature (seal) of a payload with a secret key, using the UTF8 encoding.
    /// </summary>
    public static class Utf8SealCalculator
    {
        public static readonly UTF8Encoding UTF8 = new UTF8Encoding();

        [Obsolete("CA5350 Do Not Use Weak Cryptographic Algorithms")]
        public static string HMACSHA1(string payload, string secret)
        {
            using (var alg = new HMACSHA1(UTF8.GetBytes(secret)))
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
        public static string HMACSHA256(string payload, string secret)
        {
            using (var alg = new HMACSHA256(UTF8.GetBytes(secret)))
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
        public static string HMACSHA384(string payload, string secret)
        {
            using (var alg = new HMACSHA384(UTF8.GetBytes(secret)))
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
        public static string HMACSHA512(string payload, string secret)
        {
            using (var alg = new HMACSHA512(UTF8.GetBytes(secret)))
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }

        [Obsolete("CA5350 Do Not Use Weak Cryptographic Algorithms")]
        public static string SHA1Managed(string payload)
        {
            using (var alg = new SHA1Managed())
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
       
        public static string SHA256Managed(string payload)
        {
            using (var alg = new SHA256Managed())
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
        public static string SHA384Managed(string payload)
        {
            using (var alg = new SHA384Managed())
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
        public static string SHA512Managed(string payload)
        {
            using (var alg = new SHA512Managed())
            {
                var sha = alg.ComputeHash(UTF8.GetBytes(payload));
                return ToHex(sha);
            }
        }
        public static string ToHex(byte[] ba)
        {
            if (ba == null)
            {
                throw new ArgumentNullException(nameof(ba));
            }
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
