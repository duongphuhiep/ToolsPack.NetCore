using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToolsPack.String
{
    /// <summary>
    /// Compute the signature (seal) of a payload with a secret key, using the UTF8 encoding.
    /// </summary>
    public static class Utf8SealCalculator
    {
        public static readonly UTF8Encoding UTF8 = new();

        /// <summary>
        /// Convert a string to byte[]. If the string is formatted as Hex 
        /// </summary>
        /// <param name="content">payload to convert</param>
        /// <param name="contentIsHexString">true if the payload is formatted as Hex string</param>
        /// <returns></returns>
        public static byte[] ConvertToBytesArray(string content, bool contentIsHexString)
        {
            return (contentIsHexString ? HexStringToByteArray(content) : UTF8.GetBytes(content)) ?? [];
        }

        [Obsolete("CA5350 Do Not Use Weak Cryptographic Algorithms")]
        public static string HMACSHA1(string payload, string secret, Func<byte[], string> outputBytesArrayToStringFunc, bool secretIsHexString = false, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = new HMACSHA1(ConvertToBytesArray(secret, secretIsHexString));
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }
        public static string HMACSHA256(string payload, string secret, Func<byte[], string> outputBytesArrayToStringFunc, bool secretIsHexString = false, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = new HMACSHA256(ConvertToBytesArray(secret, secretIsHexString));
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }
        public static string HMACSHA384(string payload, string secret, Func<byte[], string> outputBytesArrayToStringFunc, bool secretIsHexString = false, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = new HMACSHA384(ConvertToBytesArray(secret, secretIsHexString));
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }
        public static string HMACSHA512(string payload, string secret, Func<byte[], string> outputBytesArrayToStringFunc, bool secretIsHexString = false, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = new HMACSHA512(ConvertToBytesArray(secret, secretIsHexString));
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }

        public static string SHA1Managed(string payload, Func<byte[], string> outputBytesArrayToStringFunc, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = SHA1.Create();
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }

        public static string SHA256Managed(string payload, Func<byte[], string> outputBytesArrayToStringFunc, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = SHA256.Create();
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }
        public static string SHA384Managed(string payload, Func<byte[], string> outputBytesArrayToStringFunc, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = SHA384.Create();
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
        }
        public static string SHA512Managed(string payload, Func<byte[], string> outputBytesArrayToStringFunc, bool payloadIsHexString = false)
        {
            if (outputBytesArrayToStringFunc == null) throw new ArgumentNullException(nameof(outputBytesArrayToStringFunc));
            using var alg = SHA512.Create();
            var sha = alg.ComputeHash(ConvertToBytesArray(payload, payloadIsHexString));
            return outputBytesArrayToStringFunc(sha);
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

        public static byte[]? HexStringToByteArray(string? hexString)
        {
            if (hexString == null) return null;
            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
