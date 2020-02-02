using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Samba
{
    /// <summary>
    /// Ensure that we will only create one CifsConnectionManager per FileStorageSetting
    /// </summary>
    public static class CifsConnectionManagerFactory
    {
        static readonly ConcurrentDictionary<FileStorageSetting, CifsConnectionManager> cache = new ConcurrentDictionary<FileStorageSetting, CifsConnectionManager>();
        
        /// <summary>
        /// Create a new CifsConnectionManager and cache it to reuse next time it is called with the same FileStorageSetting.
        /// Avoid to create a new FileStorageSetting
        /// </summary>
        /// <param name="fss">FileStorageSetting</param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static CifsConnectionManager GetOrCreate(FileStorageSetting fss, ILogger log = null)
        {
            if (fss == null) 
            { 
                return null; 
            }
            if (cache.ContainsKey(fss)) 
            { 
                return cache[fss]; 
            }

            var v = new CifsConnectionManager(fss, log);
            cache[fss] = v;

            return v;
        }
    }
}