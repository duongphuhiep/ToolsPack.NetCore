using System;
using System.Security.Cryptography;

namespace Bench;

public class GenerateBase64Guid
{
    // Option 1: Simple optimization - reduce allocations
    public static string V1()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace('/', '-')
            .Replace('+', '_')
            .TrimEnd('=');  // More efficient than Replace("=", "")
    }

    // Option 2: Using Span<T> for better memory efficiency (.NET 8)
    public static string V2()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Guid.NewGuid().TryWriteBytes(guidBytes);
        
        Span<char> base64Chars = stackalloc char[24];
        Convert.TryToBase64Chars(guidBytes, base64Chars, out _);
        
        // Replace characters in-place
        for (int i = 0; i < 22; i++)  // Only process first 22 chars (skip padding)
        {
            ref char c = ref base64Chars[i];
            if (c == '/') c = '-';
            else if (c == '+') c = '_';
        }
        
        return new string(base64Chars[..22]);
    }

    // Option 4: Thread-safe high-performance version with string.Create
    public static string V4()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Guid.NewGuid().TryWriteBytes(guidBytes);
        
        return string.Create(22, guidBytes.ToArray(), static (chars, bytes) =>
        {
            Span<char> temp = stackalloc char[24];
            Convert.TryToBase64Chars(bytes, temp, out _);
            
            for (int i = 0; i < 22; i++)
            {
                char c = temp[i];
                chars[i] = c switch
                {
                    '/' => '-',
                    '+' => '_',
                    _ => c
                };
            }
        });
    }

    // Option 5: Lookup table approach (good balance of safety and performance)
    private static readonly char[] UrlSafeBase64Lookup = new char[128];
    
    static GenerateBase64Guid()
    {
        // Initialize lookup table
        for (int i = 0; i < 128; i++)
            UrlSafeBase64Lookup[i] = (char)i;
        
        UrlSafeBase64Lookup['/'] = '-';
        UrlSafeBase64Lookup['+'] = '_';
        
        for (int i = 0; i < 256; i++)
        {
            FastModulo64[i] = (uint)(i & 63);
        }
    }

    public static string V5()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Guid.NewGuid().TryWriteBytes(guidBytes);
        
        Span<char> base64Chars = stackalloc char[24];
        Convert.TryToBase64Chars(guidBytes, base64Chars, out _);
        
        var result = new char[22];
        for (int i = 0; i < 22; i++)
        {
            result[i] = UrlSafeBase64Lookup[base64Chars[i]];
        }
        
        return new string(result);
    }
    
    private static readonly char[] Base64UrlChars = 
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".ToCharArray();
    
    // Thread-local Random for thread safety without locking
    private static readonly ThreadLocal<Random> ThreadLocalRandom = 
        new(() => new Random(Guid.NewGuid().GetHashCode()));

    // Option 1: Simple Random approach (fastest for non-cryptographic use)
    public static string V6()
    {
        var random = ThreadLocalRandom.Value;
        var result = new char[22];
        
        for (int i = 0; i < 22; i++)
        {
            result[i] = Base64UrlChars[random.Next(64)];
        }
        
        return new string(result);
    }

    // Option 2: Using Span with string.Create (zero extra allocations)
    public static string V7()
    {
        return string.Create(22, ThreadLocalRandom.Value, static (chars, random) =>
        {
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = Base64UrlChars[random.Next(64)];
            }
        });
    }

    // Option 3: Cryptographically secure version
    public static string V8()
    {
        Span<byte> randomBytes = stackalloc byte[22];
        RandomNumberGenerator.Fill(randomBytes);
        
        var result = new char[22];
        for (int i = 0; i < 22; i++)
        {
            result[i] = Base64UrlChars[randomBytes[i] & 63]; // & 63 is equivalent to % 64
        }
        
        return new string(result);
    }

    // Option 4: Ultra-fast with bit manipulation (uses fewer random bytes)
    public static string V9()
    {
        // We need 22 * 6 bits = 132 bits = 17 bytes (with 4 bits unused)
        Span<byte> randomBytes = stackalloc byte[17];
        Random.Shared.NextBytes(randomBytes);
        
        var result = new char[22];
        int bitPosition = 0;
        
        for (int i = 0; i < 22; i++)
        {
            int byteIndex = bitPosition / 8;
            int bitOffset = bitPosition % 8;
            
            int value;
            if (bitOffset <= 2) // Can fit 6 bits in current byte
            {
                value = (randomBytes[byteIndex] >> bitOffset) & 63;
            }
            else // Need to span two bytes
            {
                int bitsFromFirstByte = 8 - bitOffset;
                int bitsFromSecondByte = 6 - bitsFromFirstByte;
                
                value = ((randomBytes[byteIndex] >> bitOffset) & ((1 << bitsFromFirstByte) - 1)) |
                        ((randomBytes[byteIndex + 1] & ((1 << bitsFromSecondByte) - 1)) << bitsFromFirstByte);
            }
            
            result[i] = Base64UrlChars[value];
            bitPosition += 6;
        }
        
        return new string(result);
    }
    

    // Option 6: Optimized with lookup and batch generation
    private static readonly uint[] FastModulo64 = new uint[256];
    
    public static string V11()
    {
        Span<byte> randomBytes = stackalloc byte[24]; // Round up for better alignment
        Random.Shared.NextBytes(randomBytes);
        
        return string.Create(22, randomBytes.ToArray(), static (chars, bytes) =>
        {
            for (int i = 0; i < 22; i++)
            {
                chars[i] = Base64UrlChars[FastModulo64[bytes[i]]];
            }
        });
    }
    
    public static string V12() => V12bis(Base64UrlChars);
    
    public static string V12bis(char[] possibleChars)
    {
        Span<byte> randomBytes = stackalloc byte[22];
        Random.Shared.NextBytes(randomBytes);
        
        return string.Create(22, randomBytes.ToArray(), (chars, bytes) =>
        {
            for (int i = 0; i < 22; i++)
            {
                chars[i] = possibleChars[bytes[i]%64];
            }
        });
    }
    
    
    private static readonly Random random = new Random(Guid.NewGuid().GetHashCode());

    private static readonly RNGCryptoServiceProvider rng = new();

    // Option 1: Simple and fast (recommended for most use cases)
    public static string S1()
    {
        var result = new char[22];
        
        for (int i = 0; i < 22; i++)
        {
            result[i] = Base64UrlChars[random.Next(64)];
        }
        
        return new string(result);
    }

    // Option 2: Cryptographically secure version
    public static string S2()
    {
        var randomBytes = new byte[22];
        rng.GetBytes(randomBytes);
        
        var result = new char[22];
        for (int i = 0; i < 22; i++)
        {
            result[i] = Base64UrlChars[randomBytes[i] % 64];
        }
        
        return new string(result);
    }

    // Option 3: Optimized version with better entropy usage
    public static string S3()
    {
        // We need 22 * 6 bits = 132 bits = 17 bytes minimum
        var randomBytes = new byte[22]; // Using 22 bytes for simplicity
        ThreadLocalRandom.Value.NextBytes(randomBytes);
        
        var result = new char[22];
        for (int i = 0; i < 22; i++)
        {
            // Use bitwise AND for faster modulo (64 = 2^6)
            result[i] = Base64UrlChars[randomBytes[i] & 63];
        }
        
        return new string(result);
    }

    // Option 4: Static method for convenience
    public static string S4()
    {
        var result = new char[22];
        
        for (int i = 0; i < 22; i++)
        {
            result[i] = Base64UrlChars[random.Next(64)];
        }
        
        return new string(result);
    }
    
    // Option 7: Alternative approach using StringBuilder (less efficient but sometimes preferred)
    public static string S7()
    {
        var sb = new System.Text.StringBuilder(22);
        
        for (int i = 0; i < 22; i++)
        {
            sb.Append(Base64UrlChars[random.Next(64)]);
        }
        
        return sb.ToString();
    }
    


}