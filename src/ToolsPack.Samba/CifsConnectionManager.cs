using System;
using System.IO;
using System.Net;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Samba
{
    public sealed class CifsConnectionManager
    {
        private readonly ILogger Log;
        public FileStorageSetting Config { get; private set; }
        private readonly NetworkConnection network;

        public CifsConnectionManager(FileStorageSetting cf, ILogger log = null)
        {
            if (cf is null)
            {
                throw new ArgumentNullException(nameof(cf));
            }
            
            Log = log;
            
            Config = cf;

            var networkCredential = string.IsNullOrEmpty(cf?.Domain) ?
                new NetworkCredential(cf.Login, cf.Password) :
                new NetworkCredential(cf.Login, cf.Password, cf.Domain);
            network = new NetworkConnection(cf.RemoteLocation, networkCredential, log);
        }

        /// <summary>
        /// return null if connection alive or an error string
        /// it will check existence of the file RemoteLocation\test.txt or attempt to create it
        /// if in the end the file is not existed or failed to create then it conclude that the connection is dead and return
        /// the error message about why it couldn't create the file.
        /// </summary>
        /// <returns></returns>
        public string CheckConnectionAlive()
        {
            try
            {
                var path = Path.Combine(Config.RemoteLocation, "test.txt");
                if (File.Exists(path)) 
                {
                    return null;
                }
                using (var fs = File.Create(path, 1)) { }
                return null;
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("being used by another process"))
                {
                    return null;
                }
                return ex.Message;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Log?.LogError(ex, "Failed to monitor the connection");
                return ex.Message;
            }
        }

        private readonly Channel<bool> locker = Channel.CreateBounded<bool>(1);

        /// <summary>
        /// Try to connect a number of times, throw NetworkDiskException if failed
        /// </summary>
        /// <param name="retry">number of retry</param>
        /// <returns></returns>
        public async Task Connect(int retry)
        {
            if (retry < 1)
            {
                throw new ArgumentException($"{nameof(retry)} = {retry} < 0");
            }
            try
            {
                await locker.Writer.WriteAsync(true).ConfigureAwait(false); //acquire the log

                NetworkDiskException retryConnErr = null;
                var count = 1;
                for (count = 1; count <= retry; count++)
                {
                    if (string.IsNullOrEmpty(CheckConnectionAlive()))
                    {
                        return; // connection success
                    }
                    try
                    {
                        network.Connect();
                        break; // connection success
                    }
                    catch (NetworkDiskException e)
                    {
                        retryConnErr = e;
                    }
                }

                //last chance
                var checkConnErr = CheckConnectionAlive();
                if (string.IsNullOrEmpty(checkConnErr))
                {
                    // connection success 
                    return;
                }
                else
                {
                    throw new NetworkDiskException($"retry={count} / {checkConnErr} / {retryConnErr?.Message}", retryConnErr);
                }
            }
            finally
            {
                await locker.Reader.ReadAsync().ConfigureAwait(false); //unlock
            }
        }

        /// <summary>
        /// Try to connect 3 times, throw NetworkDiskException if failed
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            await Connect(3).ConfigureAwait(false);
        }

        public void Disconnect()
        {
            network.Disconnect();
        }

        public override string ToString()
        {
            return $"network={Config.BasePath}; login={Config.Login}";
        }
    }
}