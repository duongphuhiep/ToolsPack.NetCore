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
        public static CifsConnectionManager GetOrCreate(FileStorageSetting fss, ILogger log)
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